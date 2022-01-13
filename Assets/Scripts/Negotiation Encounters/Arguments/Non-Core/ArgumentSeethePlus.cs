using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentSeethePlus : AbstractArgument
{
    public ArgumentSeethePlus(){
        this.ID = "SEETHE_PLUS";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/hotheaded");

        this.curHP = 5;
        this.maxHP = 5;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){    
        EventTurnEnd person = (EventTurnEnd) eventData;
        if (person.end == this.OWNER){
            NegotiationManager.Instance.AddAction(new DamageAction(null, TurnManager.Instance.GetOtherCharacter(this.OWNER), stacks, stacks, this));
            NegotiationManager.Instance.AddAction(new AddStacksToArgumentAction(this, 1));
        }
    }
}