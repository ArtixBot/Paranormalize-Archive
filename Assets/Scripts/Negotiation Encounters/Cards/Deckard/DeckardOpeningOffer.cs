using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardOpeningOffer : AbstractCard {

    public static string cardID = "DECKARD_OPENING_OFFER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int MIN_DAMAGE = 5;
    public int MAX_DAMAGE = 7;

    public DeckardOpeningOffer() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.ATTACK,
        new List<CardTags>{CardTags.INHERIT, CardTags.SCOUR}
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 2;
    }
}