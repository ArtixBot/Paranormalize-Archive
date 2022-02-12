using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameEvent;

public class ApplyStatusEffectAction : AbstractAction {

    private AbstractCharacter source;
    private AbstractArgument target;
    private AbstractStatusEffect effect;
    private int stacksToApply;

    ///<summary>
    ///Apply a status effect to the argument.
    ///<list type="bullet">
    ///<item><term>src</term><description>The character applying the effect.</description></item>
    ///<item><term>target</term><description>The argument receiving the effect.</description></item>
    ///<item><term>effect</term><description>The effect to apply.</description></item>
    ///<item><term>stacksToApply</term><description>The number of stacks of the effect to apply. Defaults to 0, as most status effects don't use stacks.</description></item>
    ///</list>
    ///</summary>
    public ApplyStatusEffectAction(AbstractCharacter src, AbstractArgument target, AbstractStatusEffect effect, int stacksToApply = 0){
        this.source = src;
        this.target = target;
        this.effect = effect;

        this.stacksToApply = stacksToApply;
    }

    public override int Resolve(){
        if (target == null){
            return 0;
        }
        AbstractStatusEffect existingEffect = this.target.statusEffects.FirstOrDefault(status => status.ID.Equals(this.effect.ID));

        // If the effect already exists, add stacks to it if applicable, else applying a status effect already on an argument has no effect.
        if (existingEffect != null){
            if (this.stacksToApply > 0 && existingEffect.stacks > 0){
                existingEffect.stacks += this.stacksToApply;
            }
            return 0;
        }

        AbstractStatusEffect newCopy = this.effect.MakeCopy();
        newCopy.host = target;

        this.target.statusEffects.Add(newCopy);
        newCopy.TriggerOnEffectApplied();
        EventSystemManager.Instance.TriggerEvent(new EventStatusEffectApplied(this.target, newCopy, this.stacksToApply));
        return 0;
    }
}