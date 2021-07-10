using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardSimmer : AbstractCard {

    public static string cardID = "DECKARD_SIMMER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 2;

    public DeckardSimmer() : base(
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
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentSimmer(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 2;
    }
}