using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardDomineer : AbstractCard {

    public static string cardID = "DECKARD_DOMINEER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 3;

    public int ACTIONS = 2;
    public int DRAW = 2;

    public DeckardDomineer() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.RARE,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventArgDestroyed data = (EventArgDestroyed) eventData;

        if (data.destroyingCard == this){
            NegotiationManager.Instance.AddAction(new DrawCardsAction(this.OWNER, DRAW));
            this.OWNER.curAP += this.ACTIONS;
        }
        return;
    }

    public override void Upgrade(){
        base.Upgrade();
        this.ACTIONS += 1;
        this.DRAW += 1;
    }
}