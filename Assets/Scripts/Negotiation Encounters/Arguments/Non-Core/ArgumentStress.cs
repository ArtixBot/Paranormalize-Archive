using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentStress : AbstractArgument
{
    public ArgumentStress(){
        this.ID = "STRESS";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/stress");

        this.curHP = 6;
        this.maxHP = 6;
        this.stacks = 2;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_STACKS_ADDED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){    
        EventArgStacksAdded data = (EventArgStacksAdded) eventData;
        if (data.argumentAddedTo == this && data.newStackCount >= 8){
            NegotiationManager.Instance.AddAction(new DamageAction(this.OWNER.coreArgument, this.OWNER, this.OWNER.coreArgument.curHP / 2, this.OWNER.coreArgument.curHP / 2, this));
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(this));
        }
    }
}