using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDefusal : AbstractCard {

    public static string cardID = "DECKARD_DEFUSAL";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int DRAW = 3;

    public DeckardDefusal() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        AmbienceState state = Ambience.Instance.GetState();
        if (state == AmbienceState.GUARDED || state == AmbienceState.TENSE){
            NegotiationManager.Instance.AddAction(new DrawCardsAction(source, DRAW));
        } else {
            Ambience.Instance.SetState(state - 1);
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.DRAW += 1;
    }
}