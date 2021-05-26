using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentHotheaded : AbstractArgument
{
    public ArgumentHotheaded(){
        this.ID = "HOTHEADED";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/stay-cool");

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 2;
        this.isTrait = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.AMBIENCE_STATE_SHIFT);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventAmbientStateShift data = (EventAmbientStateShift) eventData;
        if (data.newState > data.oldState){
            NegotiationManager.Instance.AddAction(new DrawCardsAction(this.OWNER, this.stacks));
        }
    }
}