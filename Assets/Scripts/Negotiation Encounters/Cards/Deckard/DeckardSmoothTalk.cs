using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardSmoothTalk : AbstractCard {

    public static string cardID = "DECKARD_SMOOTH_TALK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int STACKS = 3;
    public int INFLUENCE = 2;

    public DeckardSmoothTalk() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
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