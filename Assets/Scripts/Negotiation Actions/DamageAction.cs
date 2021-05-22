using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : AbstractAction {

    private AbstractArgument target;
    private int damageMin;
    private int damageMax;

    ///<summary>
    ///Damage an argument. Triggers an ARGUMENT_ATTACKED_BLOCKED if all damage was negated by Poise, and ARGUMENT_ATTACKED_UNBLOCKED otherwise.
    ///If the target is destroyed post-damage resolution, triggers an ARGUMENT_DESTROYED event.
    ///<list type="bullet">
    ///<item><term>target</term><description>The argument being damaged. If null, damages a random argument instead. The random target will be filtered based on argumentOwner.</description></item>
    ///<item><term>argumentOwner</term><description>The owner of the argument. This should NEVER be null!</description></item>
    ///<item><term>damageMin, damageMax</term><description>Deal [damageMin] - [damageMax] damage.</description></item>
    ///</list>
    ///</summary>
    public DamageAction(AbstractArgument target, AbstractCharacter argumentOwner, int damageMin, int damageMax){
        if (target == null){
            // TODO: Verify this random targeting works as expected!
            int range = argumentOwner.GetArguments().Count + 1;
            var rng = new System.Random();
            int index = rng.Next(range);
            this.target = (index == 0) ? argumentOwner.GetCoreArgument() : argumentOwner.GetArguments()[index - 1];
        } else {
            this.target = target;
        }
        this.damageMin = damageMin;
        this.damageMax = damageMax;
    }

    ///<returns>An integer of how much damage was dealt by this action.</returns>
    public override int Resolve(){
        int damageDealt = Random.Range(damageMin, damageMax+1);

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
        return damageDealt;
    }
}