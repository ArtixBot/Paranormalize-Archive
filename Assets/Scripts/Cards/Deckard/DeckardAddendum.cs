using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardAddendum : AbstractCard {

    public static string cardID = "DECKARD_ADDENDUM";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 1;
    public int MAX_DAMAGE = 1;
    public int STACKS = 1;

    public DeckardAddendum() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_DRAWN);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){}

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
        this.STACKS += 1;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventCardDrawn data = (EventCardDrawn) eventData;

        if (data.cardDrawn == this){
            NegotiationManager.Instance.AddAction(new DamageAction(null, TurnManager.Instance.GetOtherCharacter(data.owner), MIN_DAMAGE, MAX_DAMAGE));
            // NegotiationManager.Instance.AddAction(new DeployArgumentAction(data.owner, new ArgumentStrawman(), STACKS)); //TODO: Change to Finesse argument
        }
        return;
    }
}