using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusFortify : AbstractStatusEffect{

    public static string statusID = "FORTIFY";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusFortify() : base(statusID, statusStrings, StatusEffectType.BUFF){}
    
    public override void TriggerOnEffectApplied(){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        this.host.dmgTakenMult -= 0.5f;
    }

    public override void TriggerOnEffectExpire(){
        this.host.dmgTakenMult += 0.5f;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnStart data = (EventTurnStart) eventData;
        if (data.start == this.host.OWNER){
            this.ExpireEffect();
        }
    }

    public override AbstractStatusEffect MakeCopy(){
        return new StatusFortify();
    }
}