using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles negotiation. Also stores negotiation state variables, like current round, how many cards were played during this turn, etc.
// Singleton.
public class NegotiationManager : EventSubscriber
{
    public static readonly NegotiationManager Instance = new NegotiationManager();
    public List<AbstractAction> actionQueue = new List<AbstractAction>();

    public Ambience ambience = Ambience.Instance;
    public TurnManager tm = TurnManager.Instance;
    public EventSystemManager em = EventSystemManager.Instance;

    public AbstractCharacter player;
    public AbstractCharacter enemy;

    public int round = 1;
    public int cardsPlayedThisTurn = 0;

    // Clean up should be done in the EndNegotiationLost/EndNegotiationWon functions.
    // Get the player and enemy from the turn manager. Deep-copy their permadecks to their draw pile.
    public void StartNegotiation(){

        // TODO: REMOVE THIS, CURRENTLY FOR TESTING
        player = new PlayerDeckard();
        enemy = new TestEnemy();
        tm.AddToTurnList(player);
        tm.AddToTurnList(enemy);
        // END TODO

        em.ClearAllSubscribers();
        em.SubscribeToEvent(this, EventType.CARD_PLAYED);       // Subscribe to CARD_PLAYED event to perform all post-card play processing (ambience shift, adjusting global values, etc.)

        player = tm.GetPlayer();
        enemy = tm.GetEnemy();
        
        Debug.Log("NegotiationManager.cs: Performing deep copy of deck contents for enemy and player.");
        DeepCopyDeck(player);
        DeepCopyDeck(enemy);

        player.curAP = player.maxAP;
        enemy.curAP = enemy.maxAP;
        
        player.Draw(5);
        enemy.Draw(5);

        player.coreArgument.TriggerOnDeploy();
        enemy.coreArgument.TriggerOnDeploy();
        em.TriggerEvent(new EventTurnStart(tm.GetCurrentCharacter()));

        Debug.Log("Negotiation begins!");
    }

    public void NextTurn(){
        em.TriggerEvent(new EventTurnEnd(tm.GetCurrentCharacter()));
        tm.GetCurrentCharacter().EndTurn();        // Run end-of-turn function for current character.
        tm.NextCharacter();                        // Switch characters.
        this.cardsPlayedThisTurn = 0;
        tm.GetCurrentCharacter().StartTurn();      // Run start-of-turn function for new character.
        em.TriggerEvent(new EventTurnStart(tm.GetCurrentCharacter()));
        if (tm.GetCurrentCharacter().FACTION == FactionType.ENEMY){     // TODO: AI coding, but for now just call this function over again.
            this.NextTurn();
        }
    }

    bool currentlyResolving = false;
    public void AddAction(AbstractAction action){
        actionQueue.Add(action);
        if (this.currentlyResolving){       // While the queue is already being resolved, don't try to re-resolve it as we'll repeat calls to one action!
            return;
        } else {
            this.currentlyResolving = true;
            while (actionQueue.Count > 0){
                AbstractAction topAction = actionQueue[0];
                topAction.Resolve();
                actionQueue.RemoveAt(0);
            }
            this.currentlyResolving = false;
        }
    }

    // Game over!
    public void EndNegotiationLost(){
        Cleanup(player);
        Cleanup(enemy);
        Debug.Log("Player loses!");
        actionQueue.Clear();

        this.round = 1;
        this.cardsPlayedThisTurn = 0;
        ambience.SetState(AmbienceState.TENSE);
        // change scene to loss
    }

    // Victory!
    public void EndNegotiationWon(){
        Cleanup(player);
        Cleanup(enemy);
        Debug.Log("Player wins!");
        actionQueue.Clear();

        this.round = 1;
        this.cardsPlayedThisTurn = 0;
        ambience.SetState(AmbienceState.TENSE);
        // change scene to win
    }

    public bool PlayCard(AbstractCard card, AbstractCharacter source, AbstractArgument target){
        if (card == null) return false;
        try {
            card.Play(source, target);
            cardsPlayedThisTurn += 1;
            Debug.Log("play " + card.NAME + " on" + target.NAME);
            em.TriggerEvent(new EventCardPlayed(card, source));
            Debug.Log("post-play");
            return true;
        } catch (Exception ex){
            Debug.LogWarning("NegotiationManager.cs failed to play " +  card.NAME + ", reason: " + ex.Message);
            return false;
        }
    }

    // (Hopefully) deep copy deck contents from permadeck to drawpile, then shuffle it.
    public void DeepCopyDeck(AbstractCharacter character){
        foreach(AbstractCard card in character.permaDeck.ToList()){
            Type cardType = card.GetType();
            object copiedCard = Activator.CreateInstance(cardType);

            // This _should_ be acceptable enough for what I'm doing.
            // https://stackoverflow.com/questions/12263099/function-to-clone-an-arbitrary-object
            foreach(var property in cardType.GetProperties()){
                object originalCardValue = property.GetValue(card);
                property.SetValue(copiedCard, originalCardValue);
            }
            character.GetDrawPile().AddCard(copiedCard as AbstractCard);
        }
        character.GetDrawPile().Shuffle();
    }

    // post-negotiation cleanup helper function
    public void Cleanup(AbstractCharacter character){
        character.GetDrawPile().Clear();
        character.hand.Clear();
        character.GetDiscardPile().Clear();
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventCardPlayed data = (EventCardPlayed) eventData;
        cardsPlayedThisTurn += 1;

        if (data.cardAmbient == CardAmbient.DIALOGUE){
            ambience.AdjustState(-1);
        } else if (data.cardAmbient == CardAmbient.AGGRESSION){
            ambience.AdjustState(1);
        }
        // Debug.Log("Current ambience: " + ambience.GetState());
    }
}