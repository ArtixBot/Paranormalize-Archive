using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardPanache : AbstractCard
{
    public static string cardID = "DECKARD_PANACHE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 2;

    public DeckardPanache() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.AMBIENCE_STATE_SHIFT);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }

    private int multiplier = 0;     // done so that we can update damage values on the card itself
    public override void NotifyOfEvent(AbstractEvent eventData){
        if (eventData.type == EventType.AMBIENCE_STATE_SHIFT){
            this.MIN_DAMAGE *= 2;
            this.MAX_DAMAGE *= 2;
            this.multiplier += 1;
        }
        if (eventData.type == EventType.TURN_END){
            for (int i = 0; i < multiplier; i++){
                this.MIN_DAMAGE /= 2;
                this.MAX_DAMAGE /= 2;
            }
            this.multiplier = 0;
        }
    }
}
