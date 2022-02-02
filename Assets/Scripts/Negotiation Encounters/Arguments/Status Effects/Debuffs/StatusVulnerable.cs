using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusVulnerable : AbstractStatusEffect{

    public static string statusID = "VULNERABLE";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusVulnerable() : base(
        statusID,
        statusStrings,
        StatusEffectType.DEBUFF){
            EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
        }
    
    public override void TriggerOnEffectApplied(){
        this.host.dmgTakenMult += 0.5f;
    }

    public override void TriggerOnEffectExpire(){    // Unsubscribe from any events (if relevant). Should also undo any changes made here.
        this.host.dmgTakenMult -= 0.5f;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnEnd data = (EventTurnEnd) eventData;
        if (data.end == this.host.OWNER){
            EventSystemManager.Instance.UnsubscribeFromAllEvents(this);
            this.TriggerOnEffectExpire();
            this.host.statusEffects.Remove(this);
        }
    }
}