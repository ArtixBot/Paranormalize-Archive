using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardSmokeBreak : AbstractCard {

    public static string cardID = "DECKARD_SMOKE_BREAK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public DeckardSmokeBreak() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.RARE,
        CardType.SKILL,
        new List<CardTags>{CardTags.SCOUR}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new SetAmbienceAction(AmbienceState.GUARDED));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.COST -= 1;
    }
}