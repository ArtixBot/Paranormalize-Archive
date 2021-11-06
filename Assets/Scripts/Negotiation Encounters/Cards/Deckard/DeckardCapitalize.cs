using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardCapitalize : AbstractCard {

    public static string cardID = "DECKARD_CAPITALIZE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 4;

    private int oldCost = 1;
    private bool doubleDmg = false;

    public DeckardCapitalize() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int dbl = (doubleDmg) ? 2 : 1;
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE * dbl, MAX_DAMAGE * dbl, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.ARGUMENT_DESTROYED){
            EventArgDestroyed data = (EventArgDestroyed) eventData;
            if (data.argumentDestroyed.OWNER != this.OWNER){
                this.oldCost = this.COST;
                this.COST = 0;
                this.doubleDmg = true;
            }
        }
        if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER){
                this.COST = this.oldCost;
                this.doubleDmg = false;
            }
        }
        return;
    }
}