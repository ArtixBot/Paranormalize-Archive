using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardStonewall : AbstractCard {

    public static string cardID = "DECKARD_STONEWALL";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int POISE = 5;

    public DeckardStonewall() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, target, POISE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 3;
    }
}