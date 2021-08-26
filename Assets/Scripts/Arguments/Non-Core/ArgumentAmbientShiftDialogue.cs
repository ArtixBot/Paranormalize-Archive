using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentAmbientShiftDialogue : AbstractArgument
{
    public ArgumentAmbientShiftDialogue(){
        this.ID = "AMBIENT_SHIFT_DIALOGUE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/adaptive");

        this.curHP = 3;
        this.maxHP = 3;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (this.stacks >= 5){
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
            NegotiationManager.Instance.AddAction(new ChangeAmbienceAction(-1));
        }

        if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER) this.stacks -= 1;
        }
        if (this.stacks == 0){
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
        }
    }
}