using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNihilism : AbstractCard {

    public static string cardID = "ENEMY_NIHILISM";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int POISE = 2;

    public EnemyNihilism() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        foreach(AbstractArgument argument in source.GetTargetableArguments(true)){
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, argument, POISE));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 1;
    }
}