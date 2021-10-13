using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using GameEvent;

// Handles negotiation. Also stores negotiation state variables, like current round, how many cards were played during this turn, etc.
// Singleton.
public class NegotiationManager : EventSubscriber
{
    public static readonly NegotiationManager Instance = new NegotiationManager();
    public List<AbstractAction> actionQueue = new List<AbstractAction>();
    public List<AbstractCard> cardsPlayedThisTurn = new List<AbstractCard>();

    public Ambience ambience = Ambience.Instance;
    public TurnManager tm = TurnManager.Instance;
    public EventSystemManager em = EventSystemManager.Instance;
    public RenderNegotiation renderer;

    public AbstractCharacter player;
    public AbstractCharacter enemy;

    public int round = 1;
    public int numCardsPlayedThisTurn = 0;

    // Clean up should be done in the EndNegotiationLost/EndNegotiationWon functions.
    // Get the player and enemy from the turn manager. Deep-copy their permadecks to their draw pile.
    public void StartNegotiation(RenderNegotiation renderer){

        // TODO: REMOVE THIS, CURRENTLY FOR TESTING
        player = new PlayerDeckard();
        enemy = new TestEnemy();
        tm.AddToTurnList(player);
        tm.AddToTurnList(enemy);
        // END TODO
        this.renderer = renderer;

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
        this.numCardsPlayedThisTurn = 0;
        this.cardsPlayedThisTurn.Clear();
        
        tm.GetCurrentCharacter().StartTurn();      // Run start-of-turn function for new character.
        em.TriggerEvent(new EventTurnStart(tm.GetCurrentCharacter()));
        renderer.Redraw();

        if (tm.GetCurrentCharacter().FACTION == FactionType.ENEMY){     // TODO: AI coding, but for now just call this function over again.
            AbstractEnemy enemy = (AbstractEnemy) tm.GetCurrentCharacter();
            List<(AbstractCard, AbstractArgument)> playOrder = enemy.CalculateCardsToPlay();
            // for (int i = 0; i < playOrder.Count; i++){
            //     Debug.Log("Enemy plays " + playOrder[i].Item1 + " on " + playOrder[i].Item2 + "!");
            //     NegotiationManager.Instance.PlayCard(playOrder[i].Item1, enemy, playOrder[i].Item2);
            // }
            this.NextTurn();
        } else {
            this.round += 1;
        }
    }

    bool currentlyResolving = false;

    // If origin = null, then this action was added by a card. Else, it was added by an argument.
    // We can delete all actions currently in the queue associated w/ its argument if it gets destroyed.
    public void AddAction(AbstractAction action, AbstractArgument origin = null){
        action.origin = origin;
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
        this.numCardsPlayedThisTurn = 0;
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
        this.numCardsPlayedThisTurn = 0;
        ambience.SetState(AmbienceState.TENSE);
        // change scene to win
    }

    public bool PlayCard(AbstractCard card, AbstractCharacter source, AbstractArgument target){
        if (card == null) return false;
        try {
            card.Play(source, target);
            // numCardsPlayedThisTurn += 1;
            if (!card.suppressEventCalls){  // Event calls are suppressed here if SelectCardsFromList is invoked; instead invoked after SelectedCards is played
                em.TriggerEvent(new EventCardPlayed(card, source));
            }
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
            foreach(var field in cardType.GetFields()){
                object originalCardValue = field.GetValue(card);
                field.SetValue(copiedCard, originalCardValue);
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

    // Handle post-card playing effects (move card to discard pile, spend AP costs, etc.)
    public override void NotifyOfEvent(AbstractEvent eventData){
        EventCardPlayed data = (EventCardPlayed) eventData;
        AbstractCard cardPlayed = data.cardPlayed;
        cardsPlayedThisTurn.Add(data.cardPlayed);
        numCardsPlayedThisTurn += 1;

        if (cardPlayed.COSTS_ALL_AP){
            cardPlayed.OWNER.curAP -= cardPlayed.OWNER.curAP;   // Handle X-cost cards
        } else {
            cardPlayed.OWNER.curAP -= cardPlayed.COST;
        }
        if (cardPlayed.HasTag(CardTags.DESTROY)){         // Destroy card
            cardPlayed.OWNER.Destroy(cardPlayed);
        } else if (cardPlayed.IsTrait() || cardPlayed.HasTag(CardTags.SCOUR)){               // Scour stuff
            cardPlayed.OWNER.Scour(cardPlayed);
        } else {
            if (cardPlayed.OWNER.GetHand().Contains(cardPlayed)){           // This check is to prevent adding cards from "choice" mechanics from being added to the discard (see: Deckard's Instincts card)
                cardPlayed.OWNER.GetHand().Remove(cardPlayed);
                cardPlayed.OWNER.GetDiscardPile().AddCard(cardPlayed);
            }
        }
    }

    ///<summary>
    ///Calls upon RenderNegotiation to render a card selection menu. Returns a list of cards selected from the menu.
    ///<list type="bullet">
    ///<item><term>cardsToDisplay</term><description>The cards to be displayed in the selection menu.</description></item>
    ///<item><term>numToSelect</term><description>How many cards the user may/must select from the menu.</description></item>
    ///<item><term>mustSelectExact</term><description>If true, the user must select exactly [numToSelect] cards, else the user may select up to [numToSelect] cards.</description></item>
    ///<item><term>caller</term><description>The card calling this function.</description></item>
    ///</list>
    ///</summary>
    AbstractCard caller;
    public void SelectCardsFromList(List<AbstractCard> cardsToDisplay, int numToSelect, bool mustSelectExact, AbstractCard caller){
        this.caller = caller;
        this.caller.suppressEventCalls = true;
        if (cardsToDisplay.Count == 0 || (cardsToDisplay.Count <= numToSelect && mustSelectExact)){
            SelectedCards(cardsToDisplay);
            return;
        }
        renderer.DisplayCardSelectScreen(cardsToDisplay, numToSelect, mustSelectExact);
    }

    // Called by RenderNegotiation/NegotiationManager (and should only ever be called by those two classes)
    // Any call to SelectCardsFromList will always end up calling SelectedCards.
    public void SelectedCards(List<AbstractCard> list){
        if (this.caller != null){
            this.caller.PlayCardsSelected(list);
        } 
        em.TriggerEvent(new EventCardPlayed(caller, caller.OWNER));
        this.caller = null;
        renderer.Redraw();
    }
}