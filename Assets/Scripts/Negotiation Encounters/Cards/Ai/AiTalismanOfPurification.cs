using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTalismanOfPurification : AbstractCard
{
    public static string cardID = "AI_TALISMAN_PURIFICATION";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 1;

    public AiTalismanOfPurification() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentTalismanResolve(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
