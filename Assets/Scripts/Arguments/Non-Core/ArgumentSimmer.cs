using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentSimmer : AbstractArgument
{
    public ArgumentSimmer(){
        this.ID = "SIMMER";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/hotheaded");

        this.curHP = 6;
        this.maxHP = 6;
        this.stacks = 2;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.POISE_APPLIED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){    
        if (eventData.type == EventType.POISE_APPLIED){
            EventPoiseApplied data = (EventPoiseApplied) eventData;
            if (data.target.OWNER == this.OWNER){
                this.stacks += 1;
            }
        } else if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER){
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
            }
        } else if (eventData.type == EventType.ARGUMENT_DESTROYED){
            EventArgDestroyed data = (EventArgDestroyed) eventData;
            if (data.argumentDestroyed == this){
                AbstractCharacter enemy = TurnManager.Instance.GetOtherCharacter(this.OWNER);
                NegotiationManager.Instance.AddAction(new DamageAction(null, enemy, this.stacks, this.stacks, this));
            }
        }
    }
}