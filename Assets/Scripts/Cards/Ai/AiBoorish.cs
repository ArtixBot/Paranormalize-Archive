using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBoorish : AbstractCard
{
    public static string cardID = "AI_BOORISH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public AiBoorish() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.STARTER,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractCharacter target){
        base.Play(source, target);
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}
