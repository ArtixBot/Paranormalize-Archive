using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardBurstOfAnger : AbstractCard {

    public static string cardID = "DECKARD_BURST_OF_ANGER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int MIN_DAMAGE = 1;
    public int MAX_DAMAGE = 3;
    public int DRAW = 1;
    public int INFLUENCE = 2;

    public DeckardBurstOfAnger() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAmbientShiftAggression(), INFLUENCE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.INFLUENCE += 2;
    }
}