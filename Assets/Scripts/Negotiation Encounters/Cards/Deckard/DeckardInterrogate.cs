using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardInterrogate : AbstractCard {

    public static string cardID = "DECKARD_INTERROGATE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MULTIPLIER = 3;
    public int INFLUENCE = 4;

    public DeckardInterrogate() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.RARE,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (target.isCore || target.OWNER != this.OWNER){
            throw new Exception("You must target friendly support arguments with Interrogate!");
        }
        int damageToDeal = target.stacks * MULTIPLIER;
        AbstractCharacter enemy = TurnManager.Instance.GetOtherCharacter(target.OWNER);

        // Damage enemy arguments
        NegotiationManager.Instance.AddAction(new DamageAction(enemy.GetCoreArgument(), enemy, damageToDeal, damageToDeal, this));
        List<AbstractArgument> nonCoreArgs = enemy.GetTargetableArguments();
        foreach(AbstractArgument arg in nonCoreArgs){
            NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, damageToDeal, damageToDeal, this));
        }
        // Add/change Ambience
        if (this.isUpgraded){
            NegotiationManager.Instance.AddAction(new SetAmbienceAction(AmbienceState.DANGEROUS));
        } else {
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentAmbientShiftAggression(), INFLUENCE));
        }

        // Destroy sacrifical argument
        NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MULTIPLIER += 2;
    }
}