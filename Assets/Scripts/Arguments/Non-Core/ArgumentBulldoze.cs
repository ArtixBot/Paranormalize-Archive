using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentBulldoze : AbstractArgument
{
    public ArgumentBulldoze(){
        this.ID = "BULLDOZE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/bulldoze");

        this.stacks = 1;
        this.isTrait = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventArgDestroyed data = (EventArgDestroyed) eventData;
        if (data.argumentDestroyed.OWNER != this.OWNER){
            this.OWNER.curAP += this.stacks;
            NegotiationManager.Instance.AddAction(new DrawCardsAction(this.OWNER, this.stacks));
        }
    }
}
