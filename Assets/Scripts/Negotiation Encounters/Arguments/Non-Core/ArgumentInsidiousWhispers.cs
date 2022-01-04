using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentInsidiousWhispers : AbstractArgument
{
    public ArgumentInsidiousWhispers(){
        this.ID = "INSIDIOUS_WHISPERS";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/insidious-whispers");

        this.curHP = 2;
        this.maxHP = 2;
        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnStart data = (EventTurnStart) eventData;
        if (data.start == this.OWNER){
            NegotiationManager.Instance.AddAction(new PlantArgumentAction(TurnManager.Instance.GetOtherCharacter(this.OWNER), new ArgumentStress(), this.stacks), this);
        }
    }
}