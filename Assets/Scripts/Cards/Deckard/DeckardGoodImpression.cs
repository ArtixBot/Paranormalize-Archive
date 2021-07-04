using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardGoodImpression : AbstractCard {

    public static string cardID = "DECKARD_GOOD_IMPRESSION";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 7;

    public DeckardGoodImpression() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        this.COST += 1;
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
    }
}