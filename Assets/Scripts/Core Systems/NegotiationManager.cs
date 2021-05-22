using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles negotiation. Also stores negotiation state variables, like current round, how many cards were played during this turn, etc.
// Singleton.
public class NegotiationManager
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
        this.cardsPlayedThisTurn = 0;
        em.TriggerEvent(new EventTurnEnd(tm.GetCurrentCharacter()));
        tm.NextCharacter();
        em.TriggerEvent(new EventTurnStart(tm.GetCurrentCharacter()));
        if (tm.GetCurrentCharacter().FACTION == FactionType.ENEMY){     // TODO: AI coding, but for now just call this function over again.
            this.NextTurn();
        }
    }

    public void AddAction(AbstractAction action){
        actionQueue.Add(action);
        while (actionQueue.Count > 0){
            AbstractAction topAction = actionQueue[0];
            topAction.Resolve();
            actionQueue.RemoveAt(0);
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
        ambience.score = 0;
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
        ambience.score = 0;
        // change scene to win
    }

    public bool PlayCard(AbstractCard card, AbstractCharacter source, AbstractArgument target){
        if (card == null) return false;
        try {
            card.Play(source, target);
            cardsPlayedThisTurn += 1;
            em.TriggerEvent(new EventCardPlayed(card, source));
            return true;
        } catch (Exception ex){
            Debug.LogWarning("PlayCard.cs failed to play card, reason: " + ex.Message);
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
}