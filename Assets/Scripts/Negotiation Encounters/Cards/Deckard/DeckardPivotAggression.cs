using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This card should be created by DeckardInstincts, and not anywhere else.
public class DeckardPivotAggression : AbstractCard
{
    public static string cardID = "DECKARD_PIVOT_AGGRESSION";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int STACKS = 1;

    public DeckardPivotAggression() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.STARTER,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ChangeAmbienceAction(STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
