using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusVulnerable : AbstractStatusEffect{

    public static string statusID = "VULNERABLE";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusVulnerable() : base(statusID, statusStrings, StatusEffectType.DEBUFF){}
    
    public override void TriggerOnEffectApplied(){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        this.host.dmgTakenMult += 0.5f;
    }

    public override void TriggerOnEffectExpire(){
        this.host.dmgTakenMult -= 0.5f;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnStart data = (EventTurnStart) eventData;
        if (data.start == this.host.OWNER){
            this.ExpireEffect();
        }
    }

    public override AbstractStatusEffect MakeCopy(){
        return new StatusVulnerable();
    }
}