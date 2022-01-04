using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInsidiousWhispers : AbstractCard {

    public static string cardID = "ENEMY_INSIDIOUS_WHISPERS";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int STACKS = 1;
    public int INSTANCES = 2;

    public EnemyInsidiousWhispers() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        for (int i = 0; i < INSTANCES; i++){
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentInsidiousWhispers(), STACKS, true));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.INSTANCES += 1;
    }
}