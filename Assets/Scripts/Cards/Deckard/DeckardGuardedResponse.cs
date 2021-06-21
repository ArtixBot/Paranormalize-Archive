using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardGuardedResponse : AbstractCard {

    public static string cardID = "DECKARD_GUARDED_RESPONSE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 1;
    public int MAX_DAMAGE = 4;

    private AbstractCharacter charToApplyPoiseTo;

    public DeckardGuardedResponse() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        // I'm not sure of a better way of doing this with the current system but it  *seems* to work.
        // Essentially, before we perform the damage action (which triggers one of ARGUMENT_ATTACKED_BLOCKED/ARGUMENT_ATTACKED_UNBLOCKED), we subscribe to both events with this card.
        // The damage action on resolution will then cause this card's NotifyOfEvent() function to run, and it immediately unsubscribes the card there.
        this.charToApplyPoiseTo = source;
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_ATTACKED_BLOCKED);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.ARGUMENT_ATTACKED_UNBLOCKED);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE));
        EventSystemManager.Instance.UnsubscribeFromAllEvents(this);
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        int poiseToApply = 0;
        // need to cast to access "damageDealt" field
        if (eventData.type == EventType.ARGUMENT_ATTACKED_BLOCKED){
            EventArgAttackedBlocked data = eventData as EventArgAttackedBlocked;
            poiseToApply = data.damageDealt;
        } else {
            EventArgAttackedUnblocked data = eventData as EventArgAttackedUnblocked;
            poiseToApply = data.damageDealt;
        }
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(charToApplyPoiseTo, charToApplyPoiseTo.GetCoreArgument(), poiseToApply));
        foreach(AbstractArgument arg in charToApplyPoiseTo.GetTargetableArguments()){
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(charToApplyPoiseTo, arg, poiseToApply));
        }
    }
}