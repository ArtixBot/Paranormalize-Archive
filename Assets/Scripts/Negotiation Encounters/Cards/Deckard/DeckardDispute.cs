using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDispute : AbstractCard {

    public static string cardID = "DECKARD_DISPUTE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 1;
    public int MAX_DAMAGE = 3;
    public int STACKS = 2;

    public DeckardDispute() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.ATTACK,
        new List<CardTags>{}
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (!target.isCore){
            target.stacks -= STACKS;
            if (target.stacks <= 0){            // Destroy the argument if it falls below 0 stacks
                target.stacks = 0;
                NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target, this));
                return;
            }
        }
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
    }
}