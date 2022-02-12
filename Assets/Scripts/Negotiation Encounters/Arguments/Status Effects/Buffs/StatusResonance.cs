using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class StatusResonance : AbstractStatusEffect{

    public static string statusID = "RESONANCE";
    private static Dictionary<string, string> statusStrings = LocalizationLibrary.Instance.GetStatusStrings(statusID);

    public StatusResonance() : base(
        statusID,
        statusStrings,
        StatusEffectType.BUFF){
            EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_STATUS_EFFECT_APPLIED);
        }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventStatusEffectApplied data = (EventStatusEffectApplied) eventData;
        if (data.effectApplied.ID.Equals("RESONANCE")){     // Don't bother running through rest of event if we apply Resonance
            return;
        }
        if (data.argAppliedTo.Equals(this.host)){       // If an effect was applied to this argument (which has Resonance), spread the effect and remove Resonance
            AbstractStatusEffect checkForResonance;
            foreach(AbstractArgument arg in this.host.OWNER.GetSupportArguments()){
                checkForResonance = CheckForResonance(arg);
                if (checkForResonance != null){
                    checkForResonance.ExpireEffect();       // expire BEFORE actually spreading the effect to prevent the target argument from also triggering a resonance proc
                    NegotiationManager.Instance.AddAction(new ApplyStatusEffectAction(this.host.OWNER, arg, data.effectApplied, data.effectApplied.stacks));
                }
            }
            foreach(AbstractArgument arg in TurnManager.Instance.GetOtherCharacter(this.host.OWNER).GetSupportArguments()){
                checkForResonance = CheckForResonance(arg);
                if (checkForResonance != null){
                    checkForResonance.ExpireEffect();
                    NegotiationManager.Instance.AddAction(new ApplyStatusEffectAction(this.host.OWNER, arg, data.effectApplied, data.effectApplied.stacks));
                }
            }
            checkForResonance = CheckForResonance(this.host.OWNER.GetCoreArgument());
            if (checkForResonance != null){
                checkForResonance.ExpireEffect();
                NegotiationManager.Instance.AddAction(new ApplyStatusEffectAction(this.host.OWNER, this.host.OWNER.GetCoreArgument(), data.effectApplied, data.effectApplied.stacks));
            }

            checkForResonance = CheckForResonance(TurnManager.Instance.GetOtherCharacter(this.host.OWNER).GetCoreArgument());
            if (checkForResonance != null){
                checkForResonance.ExpireEffect();
                NegotiationManager.Instance.AddAction(new ApplyStatusEffectAction(this.host.OWNER, TurnManager.Instance.GetOtherCharacter(this.host.OWNER).GetCoreArgument(), data.effectApplied, data.effectApplied.stacks));
            }
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
}