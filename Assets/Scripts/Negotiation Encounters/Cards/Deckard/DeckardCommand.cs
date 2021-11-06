using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardCommand : AbstractCard {

    public static string cardID = "DECKARD_COMMAND";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int STACKS = 3;

    public DeckardCommand() : base(
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
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAuthority(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 2;
    }
}