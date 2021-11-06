using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardSeethe : AbstractCard {

    public static string cardID = "DECKARD_SEETHE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 1;

    public DeckardSeethe() : base(
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
        if (this.isUpgraded){
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentSeethePlus(), STACKS, true));
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentSeethePlus(), STACKS, true));
        } else {
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentSeethe(), STACKS, true));
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentSeethe(), STACKS, true));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}