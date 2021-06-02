using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardBreakthrough : AbstractCard {

    public static string cardID = "DECKARD_BREAKTHROUGH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 8;
    public int MAX_DAMAGE = 10;

    public DeckardBreakthrough() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.AMBIENCE_STATE_SHIFT);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }

    private int storeOldCost = cardCost;
    private bool costModifiedThisTurn = false;
    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.AMBIENCE_STATE_SHIFT && !costModifiedThisTurn){
            EventAmbientStateShift data = (EventAmbientStateShift) eventData;
            if (data.newState < data.oldState){
                storeOldCost = this.COST;           // Save current cost, which may be modified over the course of the negotiation
                this.COST = 0;                      // Set cost to 0.
                costModifiedThisTurn = true;        // Enable end-of-turn event
            }
        }
        if (eventData.type == EventType.TURN_END && costModifiedThisTurn){
            this.COST = storeOldCost;
            costModifiedThisTurn = false;
        }
    }
}