using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDejaVu : AbstractCard {

    public static string cardID = "DECKARD_DEJA_VU";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int DUPES = 1;

    public DeckardDejaVu() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.RARE,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (target.isCore){
            throw new Exception("Cannot target core arguments with Deja Vu!");
        }
        for (int i = 0; i < DUPES; i++){
            if (target.OWNER == source){
                NegotiationManager.Instance.AddAction(new DeployArgumentAction(target.OWNER, target, target.stacks, true));
            } else {
                NegotiationManager.Instance.AddAction(new PlantArgumentAction(target.OWNER, target, target.stacks, true));
            }
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.DUPES += 1;
    }
}