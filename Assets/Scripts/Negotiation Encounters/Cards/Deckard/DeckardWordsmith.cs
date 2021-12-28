using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardWordsmith : AbstractCard {

    public static string cardID = "DECKARD_WORDSMITH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 2;

    public DeckardWordsmith() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.UNCOMMON,
        CardType.SKILL,
        new List<CardTags>{CardTags.SCOUR}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentWordsmith(), this.STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.COST -= 1;
    }
}