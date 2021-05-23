using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardChallenge : AbstractCard {

    public static string cardID = "DECKARD_CHALLENGE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 0;
    public int MAX_DAMAGE = 0;

    public DeckardChallenge() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int poiseToRemove = source.GetCoreArgument().poise;
        source.GetCoreArgument().poise = 0;
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE + poiseToRemove, MAX_DAMAGE + poiseToRemove));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.COST -= 1;
    }
}