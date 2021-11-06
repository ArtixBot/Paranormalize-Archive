using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentCoreDesolation : AbstractArgument
{
    public ArgumentCoreDesolation(){
        this.ID = "CORE_DESOLATION";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/desolation");

        this.curHP = 200;
        this.maxHP = 200;
        this.stacks = 3;
        this.isCore = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.TURN_START){
            EventTurnStart data = (EventTurnStart) eventData;
        } else if (eventData.type == EventType.TURN_END){
            EventTurnEnd endData = (EventTurnEnd) eventData;
        }
    }
}