using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusResonance : AbstractStatusEffect{

    public static string statusID = "RESONANCE";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusResonance() : base(statusID, statusStrings, StatusEffectType.BUFF){}
    
    public override void TriggerOnEffectApplied(){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_STATUS_EFFECT_APPLIED);
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventStatusEffectApplied data = (EventStatusEffectApplied) eventData;
        if (data.effectApplied.ID.Equals("RESONANCE") || !data.argAppliedTo.Equals(this.host)){     // Don't bother running through rest of event if we apply Resonance (since Resonance can't proc Resonance), or if the effect was applied to a non-resonance arg
            return;
        }
        List<AbstractArgument> spreadEffectToTheseArguments = new List<AbstractArgument>();

        foreach(AbstractArgument arg in this.host.OWNER.GetSupportArguments()){
            if (CheckForResonance(arg) != null){
                spreadEffectToTheseArguments.Add(arg);
            }
        }
        foreach(AbstractArgument arg in TurnManager.Instance.GetOtherCharacter(this.host.OWNER).GetSupportArguments()){
            if (CheckForResonance(arg) != null){
                spreadEffectToTheseArguments.Add(arg);
            }
        }
        if (CheckForResonance(this.host.OWNER.GetCoreArgument()) != null){
            spreadEffectToTheseArguments.Add(this.host.OWNER.GetCoreArgument());
        }

        if (CheckForResonance(TurnManager.Instance.GetOtherCharacter(this.host.OWNER).GetCoreArgument()) != null){
            spreadEffectToTheseArguments.Add(TurnManager.Instance.GetOtherCharacter(this.host.OWNER).GetCoreArgument());
        }

        foreach (AbstractArgument arg in spreadEffectToTheseArguments){
            AbstractStatusEffect instance = arg.statusEffects.Find(effect => effect.ID.Equals("RESONANCE"));
            instance.ExpireEffect(); // expire resonance BEFORE actually spreading the effect to prevent the other args w/ resonance from also triggering a resonance proc   
            NegotiationManager.Instance.AddAction(new ApplyStatusEffectAction(this.host.OWNER, arg, data.effectApplied, data.effectApplied.stacks));
        }

    }

    private AbstractStatusEffect CheckForResonance(AbstractArgument arg){
        foreach(AbstractStatusEffect effect in arg.statusEffects){
            if (effect.ID.Equals("RESONANCE")){
                return effect;
            }
        }
        return null;
    }

    public override AbstractStatusEffect MakeCopy(){
        return new StatusResonance();
    }
}