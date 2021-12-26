using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBlank : AbstractCard {

    public static string cardID = "NEUTRAL_BLANK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public NeutralBlank() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.STATUS,
        CardRarity.STARTER,
        CardType.STATUS,
        new List<CardTags>{CardTags.SCOUR}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){}

    public override void Upgrade(){
        base.Upgrade();
        this.COST += 1;
    }
}