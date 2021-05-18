using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardStoic : AbstractCard {

    public static string cardID = "DECKARD_STOIC";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int POISE = 3;

    public DeckardStoic() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.STARTER,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), POISE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 2;
    }
}