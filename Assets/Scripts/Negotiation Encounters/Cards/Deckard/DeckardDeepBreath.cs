using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDeepBreath : AbstractCard {

    public static string cardID = "DECKARD_DEEP_BREATH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int POISE = 3;
    public int DRAW = 1;

    public DeckardDeepBreath() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, target, POISE));
        NegotiationManager.Instance.AddAction(new DrawCardsAction(source, DRAW));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 2;
        this.DRAW += 1;
    }
}