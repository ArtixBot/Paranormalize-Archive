using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardFollowUp : AbstractCard {

    public static string cardID = "DECKARD_FOLLOW_UP";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 2;

    public DeckardFollowUp() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.AMBIENCE_STATE_SHIFT);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MAX_DAMAGE += 2;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        AbstractCharacter char1 = TurnManager.Instance.GetPlayer();
        AbstractCharacter char2 = TurnManager.Instance.GetEnemy();
        if (char1.GetDiscardPile().ToList().Contains(this) && char1.GetHand().Count < 10){
            char1.GetHand().Add(this);
            char1.GetDiscardPile().ToList().Remove(this);
        }
        if (char2.GetDiscardPile().ToList().Contains(this) && char2.GetHand().Count < 10){
            char2.GetHand().Add(this);
            char2.GetDiscardPile().ToList().Remove(this);
        }
    }
}