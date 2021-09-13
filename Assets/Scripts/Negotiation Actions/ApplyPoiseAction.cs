using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ApplyPoiseAction : AbstractAction {

    private AbstractCharacter source;
    private AbstractArgument target;
    private int poise;
    private bool multiply;

    ///<summary>
    ///Apply Poise to an argument. Damage to an argument is removed from Poise before its resolve.
    ///<list type="bullet">
    ///<item><term>src</term><description>The character applying the Poise.</description></item>
    ///<item><term>target</term><description>The argument receiving the Poise.</description></item>
    ///<item><term>poiseToApply</term><description>The amount of Poise to apply.</description></item>
    ///<item><term>multiply</term><description>Defaults to false. If true, poiseToApply is instead a multiplicative value and will multiply the amount of existing Poise on an argument (See DeckardCalm for an example).</description></item>
    ///</list>
    ///</summary>
    public ApplyPoiseAction(AbstractCharacter src, AbstractArgument target, int poiseToApply, bool multiply = false){
        this.source = src;
        this.target = target;
        this.poise = poiseToApply;
        this.multiply = multiply;
    }

    ///<returns>Integer value zero.</returns>
    public override int Resolve(){
        if (!this.source.canGainPoise){
            this.poise = 0;
        }
        if (this.multiply){
            this.target.poise *= this.poise;
        } else {
            this.target.poise += this.poise;
        }
        if (this.poise > 0){
            EventSystemManager.Instance.TriggerEvent(new EventPoiseApplied(this.target, this.poise));
        }
        return 0;
    }
}