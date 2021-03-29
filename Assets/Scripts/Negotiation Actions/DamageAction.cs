using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage a target.
public class DamageAction : AbstractAction {

    private AbstractCharacter source;
    private AbstractArgument target;
    private int damageMin;
    private int damageMax;

    public DamageAction(AbstractCharacter source, AbstractArgument target, int damageMin, int damageMax){
        this.source = source;
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
        } else {
            this.target.poise -= damageDealt;
        }
    }
}