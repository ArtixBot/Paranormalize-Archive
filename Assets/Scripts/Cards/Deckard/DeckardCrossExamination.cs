using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardCrossExamination : AbstractCard {

    public static string cardID = "DECKARD_CROSS_EXAMINATION";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 5;
    public int MAX_DAMAGE = 6;

    public DeckardCrossExamination() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.ATTACK,
        new List<CardTags>{}
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (target.isCore){
            throw new Exception("Cannot target core arguments with cross-examination!");
        }
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }
}