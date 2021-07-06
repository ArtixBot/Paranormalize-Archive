using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardFlatter : AbstractCard {

    public static string cardID = "DECKARD_FLATTER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 3;
    private bool conditionMet = false;


    public DeckardFlatter() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_DRAWN);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (conditionMet){
            NegotiationManager.Instance.AddAction(new DamageAction(target.OWNER.GetCoreArgument(), target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
            List<AbstractArgument> nonCoreArgs = target.OWNER.GetTargetableArguments();
            foreach(AbstractArgument arg in nonCoreArgs){
                NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
            }
        } else {
            NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }

    private int storeOldCost = cardCost;
    public override void NotifyOfEvent(AbstractEvent eventData){
        List<AbstractCard> hand = this.OWNER.GetHand();
        foreach(AbstractCard card in hand){
            if (card.AMBIENCE == CardAmbient.AGGRESSION){
                conditionMet = false;
                this.COST = storeOldCost;
                return;
            }
        }
        conditionMet = true;
        if (this.COST != 0) storeOldCost = this.COST;
        this.COST = 0;
    }
}