using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardPresentTheEvidence : AbstractCard {

    public static string cardID = "DECKARD_PRESENT_THE_EVIDENCE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 2;

    public DeckardPresentTheEvidence() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (target.isCore){
            throw new Exception("Cannot target core arguments with Present the Evidence!");
        }
        target.stacks += STACKS;
        NegotiationManager.Instance.AddAction(new DamageAction(null, TurnManager.Instance.GetOtherCharacter(target.OWNER), target.stacks, target.stacks, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}