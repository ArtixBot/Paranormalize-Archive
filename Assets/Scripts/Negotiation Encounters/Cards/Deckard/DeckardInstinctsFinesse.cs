using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This card should be created by DeckardInstincts, and not anywhere else.
public class DeckardInstinctsFinesse : AbstractCard
{
    public static string cardID = "DECKARD_INSTINCTS_FINESSE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int STACKS = 1;
    public int INFLUENCE = 2;

    public DeckardInstinctsFinesse() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.STARTER,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentFinesse(), STACKS));
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAmbientShiftDialogue(), INFLUENCE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
