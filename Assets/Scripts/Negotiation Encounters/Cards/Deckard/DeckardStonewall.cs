using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardStonewall : AbstractCard {

    public static string cardID = "DECKARD_STONEWALL";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int POISE = 4;

    public DeckardStonewall() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        AmbienceState state = Ambience.Instance.GetState();
        bool bonusEffects = (state == AmbienceState.GUARDED);
        int multiplier = (bonusEffects) ? 2 : 1;
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, target, POISE * multiplier));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 2;
    }
}