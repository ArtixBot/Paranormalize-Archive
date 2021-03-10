using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDiplomacy : AbstractCard {

    public static string cardID = "DECKARD_DIPLOMACY";
    private static Dictionary<string, string> strings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static string cardName = strings["NAME"];
    private static string cardDesc = strings["DESC"];
    private static int cardCost = 1;

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