using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardBarrelThrough : AbstractCard {

    public static string cardID = "DECKARD_BARREL_THROUGH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 5;

    public DeckardBarrelThrough() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK,
        new List<CardTags>{CardTags.POISE}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int poiseSum = 0;
        poiseSum += source.GetCoreArgument().poise;
        foreach (AbstractArgument arg in source.GetTargetableArguments()){
            poiseSum += arg.poise;
        }
        poiseSum = (int)Mathf.Round((float)0.5 * poiseSum);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE + poiseSum, MAX_DAMAGE + poiseSum, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 2;
    }
}