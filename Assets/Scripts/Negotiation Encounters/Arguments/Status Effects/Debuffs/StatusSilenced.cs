using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusSilenced : AbstractStatusEffect{

    public static string statusID = "SILENCE";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusSilenced() : base(statusID, statusStrings, StatusEffectType.DEBUFF){}
    
    public override void TriggerOnEffectApplied(){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void TriggerOnEffectExpire(){
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventTurnEnd data = (EventTurnEnd) eventData;
        if (data.end == this.host.OWNER){
            this.ExpireEffect();
        }
    }

    public override AbstractStatusEffect MakeCopy(){
        return new StatusSilenced();
    }
}