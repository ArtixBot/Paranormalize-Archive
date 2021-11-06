using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardOverwhelm : AbstractCard {

    public static string cardID = "DECKARD_OVERWHELM";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int STACKS = 1;

    public DeckardOverwhelm() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.SKILL
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentOverwhelm(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}