using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentParadox : AbstractArgument
{
    public ArgumentParadox(){
        this.ID = "PARADOX";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/stay-cool");

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
        this.isTrait = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        switch (eventData.type){
            case EventType.TURN_END:
                EventTurnEnd data = (EventTurnEnd) eventData;
                if (data.end == this.OWNER){
                    this.stacks *= 2;
                }
                break;
            case EventType.ARGUMENT_DESTROYED:
                EventArgDestroyed destroyData = (EventArgDestroyed) eventData;
                if (destroyData.argumentDestroyed == this){
                    NegotiationManager.Instance.AddAction(new DamageAction(this.OWNER.GetCoreArgument(), this.OWNER, this.stacks, this.stacks));
                }
                break;
        }
    }
}