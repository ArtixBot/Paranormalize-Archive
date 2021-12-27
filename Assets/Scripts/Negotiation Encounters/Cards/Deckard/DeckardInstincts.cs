using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardInstincts : AbstractCard
{
    public static string cardID = "DECKARD_INSTINCTS";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 1;         // only does stuff for descriptions
    public int INFLUENCE = 2;      // same as above

    public DeckardInstincts() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.STARTER,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        AbstractCard a = new DeckardInstinctsFinesse();
        AbstractCard b = new DeckardInstinctsHeated();
        if (this.isUpgraded){
            a.Upgrade();
            b.Upgrade();
        }
        NegotiationManager.Instance.SelectCardsFromList(new List<AbstractCard>(){a, b}, 1, true, this);
    }

    public override void PlayCardsSelected(List<AbstractCard> selectedCards){
        foreach(AbstractCard card in selectedCards){
            NegotiationManager.Instance.PlayCard(card, this.OWNER, null);       // this adds the selected card to the discard pile, so we need to remove it from the discard pile afterwards; see next line
            // this.OWNER.GetDiscardPile().ToList().Remove(card);          // this works, but is there a more elegant solution?
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
