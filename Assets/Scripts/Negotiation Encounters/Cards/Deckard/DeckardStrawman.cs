using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardStrawman : AbstractCard {

    public static string cardID = "DECKARD_STRAWMAN";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 3;

    public DeckardStrawman() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentStrawman(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 3;
    }
}