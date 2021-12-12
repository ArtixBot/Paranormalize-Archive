using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DamageAction : AbstractAction {

    private AbstractArgument target;
    private AbstractCharacter attacker;
    private AbstractCard attackingCard;             // one of attackingCard or attackingArgument will be null. Use the non-null value to calculate damage.
    private AbstractArgument attackingArgument;
    private AbstractCharacter argumentOwner;
    private int damageMin;
    private int damageMax;
    private bool piercingDamage;

    // public DamageAction(AbstractArgument target, AbstractCharacter argumentOwner, int damageMin, int damageMax){
    //     this.target = target;
    //     this.argumentOwner = argumentOwner;
    //     this.attacker = TurnManager.Instance.GetOtherCharacter(argumentOwner);
    //     this.damageMin = damageMin;
    //     this.damageMax = damageMax;
    // }

    ///<summary>
    ///Damage an argument. Triggers an ARGUMENT_ATTACKED_BLOCKED if all damage was negated by Poise, and ARGUMENT_ATTACKED_UNBLOCKED otherwise.
    ///If the target is destroyed post-damage resolution, triggers an ARGUMENT_DESTROYED event.
    ///<list type="bullet">
    ///<item><term>target</term><description>The argument being damaged. If null, damages a random argument instead. The random target will be filtered based on argumentOwner.</description></item>
    ///<item><term>argumentOwner</term><description>The owner of the argument. If target is null, determines filter for random targeting.</description></item>
    ///<item><term>damageMin, damageMax</term><description>Deal [damageMin] - [damageMax] damage.</description></item>
    ///<item><term>attackingCard/attackingArgument</term><description>Used for damage calculations. Supply value of 'this'.</description></item>
    ///<item><term>piercingDamage</term><description>If true, damage dealt will ignore Poise on the target argument.</description></item>
    ///</list>
    ///</summary>
    public DamageAction(AbstractArgument target, AbstractCharacter argumentOwner, int damageMin, int damageMax, AbstractCard attackingCard, bool piercingDamage = false){
        this.target = target;
        this.argumentOwner = argumentOwner;
        this.attackingCard = attackingCard;
        this.damageMin = damageMin;
        this.damageMax = damageMax;
        this.piercingDamage = piercingDamage;

        this.attacker = attackingCard.OWNER;
    }

    public DamageAction(AbstractArgument target, AbstractCharacter argumentOwner, int damageMin, int damageMax, AbstractArgument attackingArgument, bool piercingDamage = false){
        this.target = target;
        this.argumentOwner = argumentOwner;
        this.attackingArgument = attackingArgument;
        this.damageMin = damageMin;
        this.damageMax = damageMax;
        this.piercingDamage = piercingDamage;

        this.attacker = attackingArgument.OWNER;
    }

    public override int Resolve(){
        // Set a random target if none exists.
        if (target == null){
            int range = argumentOwner.GetTargetableArguments().Count + 1;
            int index = UnityEngine.Random.Range(0, range);
            this.target = (index == 0) ? argumentOwner.GetCoreArgument() : argumentOwner.GetTargetableArguments()[index - 1];
        }
        int damageDealt = CalculateDamage(UnityEngine.Random.Range(damageMin, damageMax+1));
        
        // Handle Poise removal.
        if (this.target.poise > 0 && !piercingDamage){
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
            if (attackingCard != null){
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target, attackingCard));
            } else if (attackingArgument != null){
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target, attackingArgument));
            } else {
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target));
            }
        }
        return 0;
    }

    private int CalculateDamage(int initialDamage){
        int damage = initialDamage;

        // Perform damage calculations for cards.
        if (attackingCard != null){
            damage += attacker.dmgDealtAdd + attacker.dmgDealtCardAdd;          // Calculate universal damage adders.
            switch (attackingCard.AMBIENCE){                                    // Calculate ambience-specific damage adders and damage multipliers.
                case CardAmbient.INFLUENCE:
                    damage = (int)Math.Round(damage * (attacker.dmgDealtMult + attacker.dmgDealtCardMult - 1.0f));
                    break;
                case CardAmbient.AGGRESSION:
                    damage += attacker.dmgDealtAggressionAdd;
                    damage = (int)Math.Round(damage * (attacker.dmgDealtMult + attacker.dmgDealtCardMult + attacker.dmgDealtAggressionMult - 2.0f));
                    break;
                case CardAmbient.DIALOGUE:
                    damage += attacker.dmgDealtDialogueAdd;
                    damage = (int)Math.Round(damage * (attacker.dmgDealtMult + attacker.dmgDealtCardMult + attacker.dmgDealtDialogueMult - 2.0f));
                    break;
            }
            damage = (int)Math.Round((damage + target.dmgTakenAdd) * target.dmgTakenMult);              // Calculate final damage taken by the target
            // Debug.Log("Final card damage: " + damage);
            return damage;
        }

        // Perform damage calculations for arguments.
        if (attackingArgument != null){
            damage = (int)Math.Round((damage + attacker.dmgDealtAdd)* attacker.dmgDealtMult);
            damage = (int)Math.Round((damage + target.dmgTakenAdd) * target.dmgTakenMult);              // Calculate final damage taken by the target
            return damage;
        }

        // Calculate outgoing damage                                                        // since dmgDealtMult and dmgDealtCardMult are by default 1.0f, need to subtract 1.0f for a default 1.0x damage multiplier
        // damage = (int)Math.Round((damage + attacker.dmgDealtAdd + attacker.dmgDealtCardAdd) * (attacker.dmgDealtMult + attacker.dmgDealtCardMult- 1.0f));
        // // Calculate final damage taken by the target
        // damage = (int)Math.Round((damage + target.dmgTakenAdd) * target.dmgTakenMult);
        return 0;
    }
}