using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardBrash : AbstractCard {

    public static string cardID = "DECKARD_BRASH";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 2;

    public DeckardBrash() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target.OWNER.GetCoreArgument(), target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        List<AbstractArgument> nonCoreArgs = target.OWNER.GetTargetableArguments();
        foreach(AbstractArgument arg in nonCoreArgs){
            NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }
}