using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRites : AbstractCard
{
    public static string cardID = "AI_RITES";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 4;

    public AiRites() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int cnt = 0;
        foreach(AbstractArgument argument in source.GetArguments()){
            if (argument.NAME.Contains("Talisman")){
                cnt++;
            }
        }
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE + cnt, MAX_DAMAGE + cnt, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }
}
