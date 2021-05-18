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
        if (damageDealt > this.target.poise){
            // TODO: Also trigger any OnPoiseDestroy() events
            this.target.poise = 0;
            this.target.curHP -= (damageDealt - this.target.poise);

            if (this.target.curHP <= 0){        // argument is destroyed
                EventSystemManager.Instance.TriggerEvent(EventType.ARGUMENT_DESTROYED);
                this.target.TriggerOnDestroy();                             // trigger argument's on-destroy effects (if any)
                this.target.OWNER.nonCoreArguments.Remove(this.target);     // remove argument from the list of arguments (previous line will return if it's a core argument so no worries)
            }   
        } else {
            this.target.poise -= damageDealt;
        }
    }
}