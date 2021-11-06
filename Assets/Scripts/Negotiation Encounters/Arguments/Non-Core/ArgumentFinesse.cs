using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentFinesse : AbstractArgument
{
    public ArgumentFinesse(){
        this.ID = "FINESSE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/finesse");

        this.curHP = 2;
        this.maxHP = 2;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventCardPlayed data = (EventCardPlayed) eventData;
        if (data.cardAmbient == CardAmbient.DIALOGUE && data.cardType == CardType.ATTACK){
            NegotiationManager.Instance.AddAction(new DrawCardsAction(this.OWNER, 1));
            this.stacks -= 1;
        }
        if (this.stacks == 0){
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
        }
    }
}