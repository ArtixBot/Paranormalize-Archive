using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardTantrum : AbstractCard {

    public static string cardID = "DECKARD_TANTRUM";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 3;
    public int ITERATIONS = 4;

    public DeckardTantrum() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.RARE,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        for (int i = 0; i < ITERATIONS; i++){
            NegotiationManager.Instance.AddAction(new DamageAction(null, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.ITERATIONS += 1;
    }
}