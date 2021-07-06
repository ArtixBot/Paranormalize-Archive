using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentMagneticPersonality : AbstractArgument
{
    public ArgumentMagneticPersonality(){
        this.ID = "MAGNETIC_PERSONALITY";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/adaptive");

        this.curHP = 1;
        this.maxHP = 1;
        this.stacks = 2;
        this.isTrait = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.AMBIENCE_STATE_SHIFT);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentFinesse(), this.stacks));
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentHeated(), this.stacks));
    }
}