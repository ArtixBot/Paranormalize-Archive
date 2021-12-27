using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDiplomacy : AbstractCard {

    public static string cardID = "DECKARD_DIPLOMACY";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 3;

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
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 1;
    }
}