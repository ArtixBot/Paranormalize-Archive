using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardAdaptive : AbstractCard
{
    public static string cardID = "DECKARD_ADAPTIVE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 2;

    public DeckardAdaptive() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.RARE,
        CardType.TRAIT,
        new List<CardTags>{CardTags.POISE}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAdaptive(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
