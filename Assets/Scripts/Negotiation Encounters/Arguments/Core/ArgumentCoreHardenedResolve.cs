using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentCoreHardenedResolve : AbstractArgument
{
    public ArgumentCoreHardenedResolve(){
        this.ID = "CORE_HARDENED_RESOLVE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/combative");

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
        this.isCore = true;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_ATTACKED_UNBLOCKED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        switch (eventData.type){
            case EventType.TURN_START:
                EventTurnStart data = (EventTurnStart) eventData;
                if (data.start == this.OWNER){
                    this.stacks = 1;
                }
                break;
            case EventType.ARGUMENT_ATTACKED_UNBLOCKED:
                EventArgAttackedUnblocked attacked = (EventArgAttackedUnblocked) eventData;
                if (attacked.argumentAttacked == this){
                    NegotiationManager.Instance.AddAction(new ApplyPoiseAction(this.OWNER, this, this.stacks));
                    NegotiationManager.Instance.AddAction(new AddStacksToArgumentAction(this, 1));
                }
                break;
        }
    }
}