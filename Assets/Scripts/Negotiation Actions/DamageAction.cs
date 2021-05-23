﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : AbstractAction {

    private AbstractArgument target;
    private AbstractCharacter argumentOwner;
    private int damageMin;
    private int damageMax;
    private int attackXTimes;

    ///<summary>
    ///Damage an argument. Triggers an ARGUMENT_ATTACKED_BLOCKED if all damage was negated by Poise, and ARGUMENT_ATTACKED_UNBLOCKED otherwise.
    ///If the target is destroyed post-damage resolution, triggers an ARGUMENT_DESTROYED event.
    ///<list type="bullet">
    ///<item><term>target</term><description>The argument being damaged. If null, damages a random argument instead. The random target will be filtered based on argumentOwner.</description></item>
    ///<item><term>argumentOwner</term><description>The owner of the argument. Does nothing if target is null, else determines filter for random targeting.</description></item>
    ///<item><term>damageMin, damageMax</term><description>Deal [damageMin] - [damageMax] damage.</description></item>
    ///<item><term>attackXTimes</term><description>By default, 1 (attack once). If a value is supplied, attack X times equal to the value supplied to this argument.<description></item>
    ///</list>
    ///</summary>
    public DamageAction(AbstractArgument target, AbstractCharacter argumentOwner, int damageMin, int damageMax, int attackXTimes = 1){
        this.target = target;
        this.argumentOwner = argumentOwner;
        this.damageMin = damageMin;
        this.damageMax = damageMax;
        this.attackXTimes = Math.Max(attackXTimes, 1);       // if a negative/zero value is supplied ignore it!
    }

    public override int Resolve(){
        int attacks = this.attackXTimes;
        while (attacks > 0){
            // Set a random target if none exists.
            // TODO: Make random more random -- currently, a random target is selected *once*, and only once.
            // This means that random-target multiattacks will choose a random target and then perform all attacks on that one target.
            if (target == null){
                int range = argumentOwner.GetArguments().Count + 1;
                int index = UnityEngine.Random.Range(0, range);
                this.target = (index == 0) ? argumentOwner.GetCoreArgument() : argumentOwner.GetArguments()[index - 1];
            }
            int damageDealt = UnityEngine.Random.Range(damageMin, damageMax+1);
            
            // Handle Poise removal.
            if (this.target.poise > 0){
                if ( (damageDealt - this.target.poise) > 0){
                    damageDealt -= this.target.poise;
                    this.target.poise = 0;
                } else {
                    this.target.poise -= damageDealt;
                    EventSystemManager.Instance.TriggerEvent(new EventArgAttackedBlocked(target, damageDealt));
                    return damageDealt;
                }
            }

            this.target.curHP -= damageDealt;
            EventSystemManager.Instance.TriggerEvent(new EventArgAttackedUnblocked(target, damageDealt));

            // Check to see if the target argument should be destroyed.
            if (this.target.curHP <= 0){
                EventSystemManager.Instance.TriggerEvent(new EventArgDestroyed(target));    // trigger on-destroy effects (if any)
                this.target.TriggerOnDestroy();                             // Remove event subscriptions and handle victory/defeat if a core argument was destroyed
                this.target.OWNER.nonCoreArguments.Remove(this.target);     // remove argument from the list of arguments (previous line will return if it's a core argument so no worries)
            }
            attacks -= 1;
        }
        return 0;
    }
}