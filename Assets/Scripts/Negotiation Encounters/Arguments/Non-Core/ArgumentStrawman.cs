using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentStrawman : AbstractArgument
{
    public ArgumentStrawman(){
        this.ID = "STRAWMAN";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/boring");

        this.isPriorityTarget = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        this.curHP = this.stacks;
        this.maxHP = this.stacks;
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        this.maxHP = this.stacks;
    }
}
