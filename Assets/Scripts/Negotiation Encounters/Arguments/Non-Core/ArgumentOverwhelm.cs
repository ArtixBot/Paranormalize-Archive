using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentOverwhelm : AbstractArgument
{
    public ArgumentOverwhelm(){
        this.ID = "OVERWHELM";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/hotheaded");

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
            if (data.cardType == CardType.ATTACK){
                NegotiationManager.Instance.AddAction(new ApplyPoiseAction(this.OWNER, this.OWNER.GetCoreArgument(), this.stacks));
                foreach (AbstractArgument arg in this.OWNER.GetSupportArguments()){
                    NegotiationManager.Instance.AddAction(new ApplyPoiseAction(this.OWNER, arg, this.stacks));
                }
            }
        } else if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER){
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
            }
        }
    }
}