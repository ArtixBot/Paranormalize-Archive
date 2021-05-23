using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardTrashTalk : AbstractCard {

    public static string cardID = "DECKARD_TRASH_TALK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 3;
    public int ITERATIONS = 2;

    public DeckardTrashTalk() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        for (int i = 0; i < this.ITERATIONS; i++){
            NegotiationManager.Instance.AddAction(new DamageAction(null, target.OWNER, MIN_DAMAGE, MAX_DAMAGE));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.ITERATIONS += 1;
    }
}