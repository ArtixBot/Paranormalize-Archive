using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentAuthority : AbstractArgument
{
    public ArgumentAuthority(){
        this.ID = "AUTHORITY";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/authority");

        this.curHP = 10;
        this.maxHP = 10;
        this.stacks = 2;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        this.OWNER.drawMod += 1;
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.POISE_APPLIED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void TriggerOnDestroy(){
        base.TriggerOnDestroy();
        this.OWNER.drawMod -= 1;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.POISE_APPLIED){
            EventPoiseApplied data = (EventPoiseApplied) eventData;
            if (data.target == this){
                AbstractCharacter enemy = TurnManager.Instance.GetOtherCharacter(this.OWNER);
                NegotiationManager.Instance.AddAction(new DamageAction(null, enemy, this.stacks, this.stacks, this));
            }
        } else if (eventData.type == EventType.TURN_END){
            EventTurnEnd data = (EventTurnEnd) eventData;
            if (data.end == this.OWNER){
                AbstractCharacter enemy = TurnManager.Instance.GetOtherCharacter(this.OWNER);
                NegotiationManager.Instance.AddAction(new DamageAction(null, enemy, this.stacks, this.stacks, this));
            }
        }
    }
}