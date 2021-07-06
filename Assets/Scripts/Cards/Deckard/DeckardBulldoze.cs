using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardBulldoze : AbstractCard {

    public static string cardID = "DECKARD_BULLDOZE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int STACKS = 1;

    public DeckardBulldoze() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.RARE,
        CardType.TRAIT
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentBulldoze(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}