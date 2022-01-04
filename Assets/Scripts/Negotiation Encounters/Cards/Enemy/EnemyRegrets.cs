using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegrets : AbstractCard {

    public static string cardID = "ENEMY_REGRETS";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int STACKS = 3;

    public EnemyRegrets() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new PlantArgumentAction(TurnManager.Instance.GetOtherCharacter(this.OWNER), new ArgumentStress(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}