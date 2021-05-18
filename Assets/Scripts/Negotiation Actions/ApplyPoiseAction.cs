using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPoiseAction : AbstractAction {

    private AbstractCharacter source;
    private AbstractArgument target;
    private int poise;

    ///<summary>
    ///Apply Poise to an argument. Damage to an argument is removed from Poise before its resolve.
    ///<list type="bullet">
    ///<item><term>src</term><description>The character applying the Poise.</description></item>
    ///<item><term>target</term><description>The argument receiving the Poise.</description></item>
    ///<item><term>poiseToApply</term><description>The amount of Poise to apply.</description></item>
    ///</list>
    ///</summary>
    public ApplyPoiseAction(AbstractCharacter src, AbstractArgument target, int poiseToApply){
        this.source = src;
        this.target = target;
        this.poise = poiseToApply;
    }

    public override void Resolve(){
        this.target.poise += this.poise;
    }
}