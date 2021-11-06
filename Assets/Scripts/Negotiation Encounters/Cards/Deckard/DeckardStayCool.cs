using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardStayCool : AbstractCard
{
    public static string cardID = "DECKARD_STAY_COOL";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 2;

    public DeckardStayCool() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.TRAIT
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentStayCool(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
