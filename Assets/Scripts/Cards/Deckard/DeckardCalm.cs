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
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);

        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), POISE));
        source.GetCoreArgument().poise *= 2;

        foreach(AbstractArgument arg in source.GetArguments()){
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, arg, POISE));
            arg.poise *= 2;
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 1;
    }
}