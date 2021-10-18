using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardReversal : AbstractCard {

    public static string cardID = "DECKARD_REVERSAL";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;
    
    public DeckardReversal() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int poiseToSteal = target.poise;
        target.poise = 0;
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), poiseToSteal));
        if (this.isUpgraded){
            foreach (AbstractArgument arg in source.GetArguments()){
                NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, arg, poiseToSteal));
            }
        }
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}