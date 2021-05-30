using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardRuminate : AbstractCard
{
    public static string cardID = "DECKARD_RUMINATE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public DeckardRuminate() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        List<AbstractCard> selectedCard = NegotiationManager.Instance.SelectCardsFromList(source.GetHand(), 1, true);
    }

    public override void Upgrade(){
        base.Upgrade();
        this.COST -= 1;
    }
}
