using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardGrandiosity : AbstractCard {

    public static string cardID = "DECKARD_GRANDIOSITY";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 5;

    public int MIN_DAMAGE = 8;
    public int MAX_DAMAGE = 12;

    public DeckardGrandiosity() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.RARE,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int count = source.GetArguments().Count;
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 4;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        if (this.OWNER.GetHand().Contains(this)){
            int modifier = this.OWNER.GetArguments().Count;
            this.COST = cardCost - modifier;
        }
    }
}