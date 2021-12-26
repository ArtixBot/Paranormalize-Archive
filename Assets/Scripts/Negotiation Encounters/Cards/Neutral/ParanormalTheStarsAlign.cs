using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParanormalTheStarsAlign : AbstractCard {

    public static string cardID = "PARANORMAL_THE_STARS_ALIGN";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 4;
    public int MAX_DAMAGE = 6;

    public ParanormalTheStarsAlign() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.PARANORMAL,
        CardRarity.UNIQUE,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target.OWNER.GetCoreArgument(), target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        List<AbstractArgument> nonCoreArgs = target.OWNER.GetTargetableArguments();
        foreach(AbstractArgument arg in nonCoreArgs){
            NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }

        AbstractCharacter otherCharacter = TurnManager.Instance.GetOtherCharacter(target.OWNER);
        NegotiationManager.Instance.AddAction(new DamageAction(otherCharacter.GetCoreArgument(), otherCharacter, MIN_DAMAGE, MAX_DAMAGE, this));
        nonCoreArgs = otherCharacter.GetTargetableArguments();
        foreach(AbstractArgument arg in nonCoreArgs){
            NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }
}