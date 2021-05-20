using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : AbstractAction {

    private AbstractArgument target;
    private int damageMin;
    private int damageMax;

    ///<summary>
    ///Damage an argument. Triggers an ARGUMENT_DESTROYED event if the target is destroyed post-damage resolution.
    ///<list type="bullet">
    ///<item><term>target</term><description>The argument being damaged.</description></item>
    ///<item><term>damageMin, damageMax</term><description>Deal [damageMin] - [damageMax] damage.</description></item>
    ///</list>
    ///</summary>
    public DamageAction(AbstractArgument target, int damageMin, int damageMax){
        this.target = target;
        this.damageMin = damageMin;
        this.damageMax = damageMax;
    }

    public override void Resolve(){
        int damageDealt = Random.Range(damageMin, damageMax+1);

        // Handle Poise removal.
        if (this.target.poise > 0){
            if ( (damageDealt - this.target.poise) > 0){
                damageDealt -= this.target.poise;
                this.target.poise = 0;
            } else {
                this.target.poise -= damageDealt;
                EventSystemManager.Instance.TriggerEvent(new EventArgAttackedBlocked(target, damageDealt));
                return;
            }
        }

        this.target.curHP -= damageDealt;
        EventSystemManager.Instance.TriggerEvent(new EventArgAttackedUnblocked(target, damageDealt));

        // Check to see if the target argument should be destroyed.
        if (this.target.curHP <= 0){
            EventSystemManager.Instance.TriggerEvent(new EventArgDestroyed(target));    // trigger on-destroy effects (if any)
            this.target.TriggerOnDestroy();                             // Remove event subscriptions and handle victory/defeat if a core argument was destroyed
            this.target.OWNER.nonCoreArguments.Remove(this.target);     // remove argument from the list of arguments (previous line will return if it's a core argument so no worries)
            return;
        }
    }
}