using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardOverbear : AbstractCard {

    public static string cardID = "DECKARD_OVERBEAR";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 2;

    public DeckardOverbear() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.SKILL
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int multiplier = 1 + source.nonCoreArguments.Count;     // 1 for core argument
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE * multiplier, MAX_DAMAGE * multiplier, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }
}