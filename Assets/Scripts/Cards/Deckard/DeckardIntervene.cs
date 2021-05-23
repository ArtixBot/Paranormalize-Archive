using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardIntervene : AbstractCard {

    public static string cardID = "DECKARD_INTERVENE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int POISE = 2;

    public DeckardIntervene() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, target, POISE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 1;
    }
}