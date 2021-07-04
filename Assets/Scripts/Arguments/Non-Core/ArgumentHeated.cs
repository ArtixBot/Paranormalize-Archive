using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentHeated : AbstractArgument
{
    public ArgumentHeated(){
        this.ID = "HEATED";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/hotheaded");

        this.curHP = 2;
        this.maxHP = 2;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    int currentlyApplying = 0;
    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER) stacks -= 1;
        }
        if (this.stacks != currentlyApplying){
            int delta = (this.stacks - currentlyApplying);
            this.OWNER.dmgDealtAggressionAdd += delta;
            currentlyApplying = this.stacks;
        }
        if (this.stacks == 0){
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
        }
    }
}