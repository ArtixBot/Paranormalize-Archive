using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardMagneticPersonality : AbstractCard
{
    public static string cardID = "DECKARD_MAGNETIC_PERSONALITY";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 3;

    public int STACKS = 2;

    public DeckardMagneticPersonality() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.RARE,
        CardType.TRAIT
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentMagneticPersonality(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}
