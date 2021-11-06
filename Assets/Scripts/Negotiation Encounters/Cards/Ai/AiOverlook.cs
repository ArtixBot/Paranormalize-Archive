using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiOverlook : AbstractCard
{
    public static string cardID = "AI_OVERLOOK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int poise = 2;

    public AiOverlook() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.STARTER,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, target, poise));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.poise += 2;
    }
}
