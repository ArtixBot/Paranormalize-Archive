using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckardRackTheBrain : AbstractCard {

    public static string cardID = "DECKARD_RACK_THE_BRAIN";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int CHOICES = 3;

    public DeckardRackTheBrain() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.UNCOMMON,
        CardType.SKILL,
        new List<CardTags>{CardTags.SCOUR}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        var rnd = new System.Random();
        List<AbstractCard> cards = source.GetDrawPile().deck.OrderBy(x => rnd.Next()).Take(CHOICES).ToList();       // Efficient? Not particularly. But draw piles will probably only ever range to at most 50 cards anyways.
        NegotiationManager.Instance.SelectCardsFromList(cards, 1, true, this);
    }

    public override void PlayCardsSelected(List<AbstractCard> selectedCards){
        AbstractCard card = selectedCards[0];
        if (card != null){
            this.OWNER.GetDrawPile().RemoveCard(card);
            this.OWNER.GetHand().Add(card);
            card.COST = 0;
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.CHOICES += 2;
    }
}