using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentSeethe : AbstractArgument
{
    public ArgumentSeethe(){
        this.ID = "SEETHE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/hotheaded");

        this.curHP = 4;
        this.maxHP = 4;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){    
        EventTurnStart person = (EventTurnStart) eventData;
        if (person.start == this.OWNER){
            NegotiationManager.Instance.AddAction(new DamageAction(null, TurnManager.Instance.GetOtherCharacter(this.OWNER), stacks, stacks, this));
        }
    }
}