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
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){    
        EventTurnEnd person = (EventTurnEnd) eventData;
        if (person.end == this.OWNER){
            AbstractCharacter opponent = TurnManager.Instance.GetOtherCharacter(this.OWNER);
            NegotiationManager.Instance.AddAction(new DamageAction(null, opponent, stacks, stacks, this));
        }
    }
}