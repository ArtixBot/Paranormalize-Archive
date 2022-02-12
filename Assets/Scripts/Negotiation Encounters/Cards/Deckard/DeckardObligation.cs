using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardObligation : AbstractCard {

    public static string cardID = "DECKARD_OBLIGATION";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 4;
    public int MAX_DAMAGE = 8;
    public int STACKS = 1;

    public DeckardObligation() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.RARE,
        CardType.ATTACK
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        foreach(AbstractArgument support in source.GetSupportArguments()){
            NegotiationManager.Instance.AddAction(new AddStacksToArgumentAction(support, this.STACKS));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 4;
        this.STACKS += 1;
    }
}