using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentChatterbox : AbstractArgument
{
    public ArgumentChatterbox(){
        this.ID = "CHATTERBOX";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/chatterbox");

        this.curHP = 4;
        this.maxHP = 4;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnEnd data = (EventTurnEnd) eventData;
        if (data.end == this.OWNER){
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
        }
    }
}
