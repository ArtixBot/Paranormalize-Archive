using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardCalm : AbstractCard {

    public static string cardID = "DECKARD_CALM";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int POISE = 1;

    public DeckardCalm() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.STARTER,
        CardType.SKILL,
        new List<CardTags>{CardTags.POISE}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);

        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), POISE));
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), 2, true));

        foreach(AbstractArgument arg in source.GetArguments()){
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, arg, POISE));
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, arg, 2, true));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 1;
    }
}