using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage a target.
public class ApplyPoiseAction : AbstractAction {

    private AbstractCharacter source;
    private AbstractArgument target;
    private int poise;

    public ApplyPoiseAction(AbstractCharacter source, AbstractArgument target, int poiseToApply){
        this.source = source;
        this.target = target;
        this.poise = poiseToApply;
    }

    public override void Resolve(){
        this.target.poise += this.poise;
    }
}