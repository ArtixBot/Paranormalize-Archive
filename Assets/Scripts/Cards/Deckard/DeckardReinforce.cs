using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardReinforce : AbstractCard {

    public static string cardID = "DECKARD_REINFORCE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    private float multiplier = 0.5f;

    public DeckardReinforce() : base(
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
        target.stacks += (int)Math.Round(target.poise * multiplier);
        target.poise = 0;
    }

    public override void Upgrade(){
        base.Upgrade();
        this.multiplier += 0.5f;
    }
}