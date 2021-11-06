using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBoorish : AbstractCard
{
    public static string cardID = "AI_BOORISH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 1;
    public int MAX_DAMAGE = 4;

    public AiBoorish() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.STARTER,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
    }
}
