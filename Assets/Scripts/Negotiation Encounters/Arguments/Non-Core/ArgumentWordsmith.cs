using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentWordsmith : AbstractArgument
{
    public ArgumentWordsmith(){
        this.ID = "WORDSMITH";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/composed");

        this.curHP = 3;
        this.maxHP = 3;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.CARD_PLAYED){
            EventCardPlayed data = (EventCardPlayed) eventData;
            if (data.cardPlayed.AMBIENCE == CardAmbient.DIALOGUE){
                NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentAmbientShiftDialogue(), this.stacks));
            } else if (data.cardPlayed.AMBIENCE == CardAmbient.AGGRESSION){
                NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentAmbientShiftAggression(), this.stacks));
            }
        } else if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER){
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
            }
        }
    }
}