using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine;
using GameEvent;

public enum RewardType {NORMAL, ELITE, BOSS};
// Handles negotiation. Also stores negotiation state variables, like current round, how many cards were played during this turn, etc.
// Singleton.
public class NegotiationManager : EventSubscriber {
    public static readonly NegotiationManager Instance = new NegotiationManager();
    public List<AbstractAction> actionQueue = new List<AbstractAction>();
    public List<AbstractCard> cardsPlayedThisTurn = new List<AbstractCard>();

    public static TurnManager tm = TurnManager.Instance;
    public static EventSystemManager em = EventSystemManager.Instance;
    
    public Ambience ambience = Ambience.Instance;
    public RenderNegotiation renderer;

    public AbstractCharacter player;
    public AbstractEnemy enemy;

    public int round = 1;
    public int numCardsPlayedThisTurn = 0;
    public int argumentsDeployedThisNegotiation = 0;

    private RewardType rewardsOnWin;

    // Clean up should be done in the EndNegotiationLost/EndNegotiationWon functions.
    // Get the player and enemy from the turn manager. Deep-copy their permadecks to their draw pile.
    public void StartNegotiation(RenderNegotiation renderer, AbstractEnemy enemyChar, RewardType rewardsOnWin = RewardType.NORMAL){
        this.player = (GameState.mainChar == null) ? new PlayerDeckard() : GameState.mainChar;    // null check for player - if null, use Deckard as base
        this.enemy = (enemyChar == null) ? new TestEnemy() : this.enemy;             // null check for enemy - if null, use TestEnemy as base
        this.rewardsOnWin = rewardsOnWin;
        tm.AddToTurnList(player);
        tm.AddToTurnList(enemy);

        this.renderer = renderer;

        em.ClearAllSubscribers();
        em.SubscribeToEvent(this, EventType.CARD_PLAYED);       // Subscribe to CARD_PLAYED event to perform all post-card play processing (ambience shift, adjusting global values, etc.)
        em.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);// Subscribe to ARGUMENT_DESTROYED event to perform enemy intent recalculations whenever an argument is destroyed
        
        Debug.Log("NegotiationManager.cs: Performing deep copy of deck contents for enemy and player.");
        DeepCopyDeck(player);
        DeepCopyDeck(enemy);

        this.player.curAP = player.maxAP;
        this.enemy.curAP = enemy.maxAP;
        this.enemy.coreArgument.curHP = this.enemy.coreArgument.maxHP;
        
        player.Draw(5);
        enemy.Draw(5);

        player.coreArgument.TriggerOnDeploy();
        enemy.coreArgument.TriggerOnDeploy();
        em.TriggerEvent(new EventTurnStart(tm.GetCurrentCharacter()));

        Debug.Log("Negotiation begins!");
        
        enemy.CalculateIntents();
        // foreach(EnemyIntent intent in enemy.intents){
        //     Debug.Log($"The enemy intends to play {intent.cardToPlay} on {intent.argumentTargeted}");
        // }
    }

    public void NextTurn(){
        em.TriggerEvent(new EventTurnEnd(tm.GetCurrentCharacter()));
        tm.GetCurrentCharacter().EndTurn();        // Run end-of-turn function for current character.
        
        tm.NextCharacter();                        // Switch characters.
        this.numCardsPlayedThisTurn = 0;
        this.cardsPlayedThisTurn.Clear();
        
        tm.GetCurrentCharacter().StartTurn();      // Run start-of-turn function for new character.
        em.TriggerEvent(new EventTurnStart(tm.GetCurrentCharacter()));

        if (tm.GetCurrentCharacter().FACTION == FactionType.ENEMY){     // TODO: AI coding, but for now just call this function over again.
            foreach (EnemyIntent intent in enemy.intents){
                intent.Resolve();
            }
            this.NextTurn();
        } else {
            AbstractEnemy enemy = (AbstractEnemy) tm.GetOtherCharacter(tm.GetCurrentCharacter());
            enemy.CalculateIntents();
            foreach(EnemyIntent intent in enemy.intents){
                Debug.Log($"The enemy intends to play {intent.cardToPlay} on {intent.argumentTargeted}");
            }
            renderer.Redraw();
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

        tm.GetTurnList().Clear();
        this.round = 1;
        this.numCardsPlayedThisTurn = 0;
        this.argumentsDeployedThisNegotiation = 0;
        ambience.SetState(AmbienceState.TENSE);
        this.currentlyResolving = false;

        // TODO: Better end-of-negotiation handling, but for now return to Overworld
        // renderer.EndNegotiationRender();
    }

    // Victory!
    public void EndNegotiationWon(){
        Cleanup(player);
        Cleanup(enemy);
        Debug.Log("Player wins!");

        // cleanup negotiation state
        tm.GetTurnList().Clear();
        this.round = 1;
        this.numCardsPlayedThisTurn = 0;
        this.argumentsDeployedThisNegotiation = 0;
        ambience.SetState(AmbienceState.TENSE);
        this.currentlyResolving = false;

        // allocate rewards to the player
        AllocateRewards(rewardsOnWin);

        // TODO: Better end-of-negotiation handling, but for now return to Overworld
        // renderer.EndNegotiationRender();
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
                // Debug.Log(field + ": " + originalCardValue);
                field.SetValue(copiedCard, originalCardValue);
            }
            character.GetDrawPile().AddCard(copiedCard as AbstractCard);
        }
        character.GetDrawPile().Shuffle();
    }

    // post-negotiation cleanup helper function
    public void Cleanup(AbstractCharacter character){
        actionQueue.Clear();
        character.coreArgument.poise = 0;
        character.GetHand().Clear();
        character.GetArguments().Clear();
        character.GetDrawPile().Clear();
        character.GetDiscardPile().Clear();
        character.GetScourPile().Clear();
    }

    // Handle post-card playing effects (move card to discard pile, spend AP costs, etc.)
    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.CARD_PLAYED){
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
        } else if (eventData.type == EventType.ARGUMENT_DESTROYED){
            EventArgDestroyed data = (EventArgDestroyed) eventData;
            enemy.RecalculateIntents(data.argumentDestroyed);
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

    // Give rewards to the player on victory
    private void AllocateRewards(RewardType type){
        List<AbstractCard> drafts = new List<AbstractCard>();
        int money = 0;
        int mastery = 0;

        switch (type){
            case RewardType.NORMAL:
                // Each draft is 70% C, 25% UC, 5% R. 
                for (int i = 0; i < 3; i++){
                    int rng = UnityEngine.Random.Range(0, 100);
                    if (0 <= rng && rng < 70){
                        drafts.Add(DraftCard(CardRarity.COMMON));
                    } else if (rng < 95){
                        drafts.Add(DraftCard(CardRarity.UNCOMMON));
                    } else {
                        drafts.Add(DraftCard(CardRarity.RARE));
                    }
                }
                money += (int)Math.Round(UnityEngine.Random.Range(20, 40) * GameState.moneyGainMod);
                break;
            case RewardType.ELITE:
                // Each draft is 80% UC, 20% R.
                for (int i = 0; i < 3; i++){
                    int rng = UnityEngine.Random.Range(0, 100);
                    if (0 <= rng && rng < 80){
                        drafts.Add(DraftCard(CardRarity.UNCOMMON));
                    } else {
                        drafts.Add(DraftCard(CardRarity.RARE));
                    }
                }
                money += (int)Math.Round(UnityEngine.Random.Range(40, 60) * GameState.moneyGainMod);
                mastery += 1;
                break;
            case RewardType.BOSS:
                for (int i = 0; i < 3; i++){
                    drafts.Add(DraftCard(CardRarity.RARE));   // Each draft is 100% R.
                }
                money += (int)Math.Round(UnityEngine.Random.Range(90, 110) * GameState.moneyGainMod);
                mastery += 1;
                break;
        }
        GameState.money += money;
        GameState.mastery += mastery;
        renderer.DisplayVictoryScreen(drafts, money, mastery);
    }

    private AbstractCard DraftCard(CardRarity rarity){
        List<AbstractCard> filteredList = CardLibrary.Instance.Lookup(GameState.mainChar).Where(card => card.RARITY == rarity).ToList();
        AbstractCard draft = filteredList[UnityEngine.Random.Range(0, filteredList.Count)];
        int rng = UnityEngine.Random.Range(0, 100);
        if (rng < 20){      // 20% chance to upgrade a drafted card
            draft.Upgrade();
        }
        return draft;
    }
}