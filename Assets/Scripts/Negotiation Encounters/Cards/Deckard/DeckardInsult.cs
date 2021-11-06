using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardInsult : AbstractCard
{
    public static string cardID = "DECKARD_INSULT";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 5;
    public int INFLUENCE = 2;

    public DeckardInsult() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAmbientShiftAggression(), INFLUENCE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 2;
        this.INFLUENCE += 1;
    }
}
