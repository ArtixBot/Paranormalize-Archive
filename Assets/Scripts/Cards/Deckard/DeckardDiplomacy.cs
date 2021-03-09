using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDiplomacy : AbstractCard {

    public static string cardID = "DECKARD_DIPLOMACY";
    private static string cardName = "Diplomacy";
    private static string cardDesc = "Deal 1-3 damage.";
    private static int cardCost = 3;

    public DeckardDiplomacy() : base(
        cardID,
        cardName,
        cardCost,
        CardRarity.STARTER,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractCharacter target){
        base.Play(source, target);
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}