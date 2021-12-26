using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardFlatter : AbstractCard {

    public static string cardID = "DECKARD_FLATTER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 3;

    public DeckardFlatter() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (source.GetArgument(new ArgumentFinesse()) != null){
            NegotiationManager.Instance.AddAction(new DamageAction(target.OWNER.GetCoreArgument(), target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
            List<AbstractArgument> nonCoreArgs = target.OWNER.GetTargetableArguments();
            foreach(AbstractArgument arg in nonCoreArgs){
                NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
            }
        } else {
            NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }
}