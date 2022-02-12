using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusFortify : AbstractStatusEffect{

    public static string statusID = "FORTIFY";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusFortify() : base(
        statusID,
        statusStrings,
        StatusEffectType.BUFF){
            EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        }
    
    public override void TriggerOnEffectApplied(){
        this.host.dmgTakenMult -= 0.5f;
    }

    public override void TriggerOnEffectExpire(){    // Unsubscribe from any events (if relevant). Should also undo any changes made here.
        this.host.dmgTakenMult += 0.5f;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnStart data = (EventTurnStart) eventData;
        if (data.start == this.host.OWNER){
            this.ExpireEffect();
        }
    }
}