using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCardVulnerable : AbstractCard {

    public static string cardID = "TEST_CARD_VULNERABLE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public TestCardVulnerable() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.STATUS,
        CardRarity.STARTER,
        CardType.STATUS
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        NegotiationManager.Instance.AddAction(new ApplyStatusEffectAction(source, target, new StatusVulnerable()));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.COST += 1;
    }
}