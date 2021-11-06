using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentInept : AbstractArgument
{
    public ArgumentInept(){
        this.ID = "INEPT";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/bad-meme");

        this.curHP = 8;
        this.maxHP = 8;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
        this.OWNER.canGainPoise = false;
    }

    public override void TriggerOnDestroy(){
        base.TriggerOnDestroy();
        this.OWNER.canGainPoise = true;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER) this.stacks -= 1;
        }
        if (this.stacks == 0){
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
        }
    }
}