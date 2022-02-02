using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardTheAce : AbstractCard {

    public static string cardID = "DECKARD_THE_ACE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 4;
    public int MAX_DAMAGE = 6;

    public DeckardTheAce() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.RARE,
        CardType.ATTACK,
        new List<CardTags>{CardTags.SCOUR}
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_DESTROYED);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventArgDestroyed data = (EventArgDestroyed) eventData;

        if (data.destroyingCard == this){
            GameState.mastery += 1;
        }
        return;
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }
}