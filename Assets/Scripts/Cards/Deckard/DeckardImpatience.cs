using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardImpatience : AbstractCard
{
    public static string cardID = "DECKARD_IMPATIENCE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public DeckardImpatience() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.RARE,
        CardType.SKILL,
        new List<CardTags>{CardTags.SCOUR}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        this.OWNER.curAP += target.stacks;
        NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.COST -= 1;
    }
}
