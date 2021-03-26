using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDiplomacy : AbstractCard {

    public static string cardID = "DECKARD_DIPLOMACY";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public DeckardDiplomacy() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.STARTER,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}