using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardInsight : AbstractCard
{
    public static string cardID = "DECKARD_INSIGHT";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int DRAW = 2;
    public int POISE = 2;

    public DeckardInsight() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DrawCardsAction(source, DRAW));
        if (this.isUpgraded){
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), POISE));

            foreach(AbstractArgument arg in source.GetTargetableArguments()){
                NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, arg, POISE));
            }
        }
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}
