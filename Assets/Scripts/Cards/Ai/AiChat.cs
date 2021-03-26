using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChat : AbstractCard
{
    public static string cardID = "AI_CHAT";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public AiChat() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.STARTER,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}
