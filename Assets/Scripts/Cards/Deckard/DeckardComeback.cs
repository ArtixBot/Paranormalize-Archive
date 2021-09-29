using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardComeback : AbstractCard {

    public static string cardID = "DECKARD_COMEBACK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 3;

    public DeckardComeback() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    AbstractCharacter temp;
    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        temp = source;
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        NegotiationManager.Instance.SelectCardsFromList(source.GetDiscardPile().ToList(), 1, true, this);
    }

    public override void PlayCardsSelected(List<AbstractCard> selectedCards){
        for(int i = 0; i < selectedCards.Count; i++){
            AbstractCard card = selectedCards[i];
            temp.GetHand().Add(card);
            temp.GetDiscardPile().RemoveCard(card);
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }
}