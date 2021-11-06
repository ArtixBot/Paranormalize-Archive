using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardKnack : AbstractCard {

    public static string cardID = "DECKARD_KNACK";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int DRAW = 2;

    public DeckardKnack() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.SKILL
        // new List<CardTags>{CardTags.FINESSE}
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_DRAWN);
        NegotiationManager.Instance.AddAction(new DrawCardsAction(source, DRAW));
        EventSystemManager.Instance.UnsubscribeFromAllEvents(this);
    }

    public override void Upgrade(){
        base.Upgrade();
        this.DRAW += 1;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventCardDrawn data = (EventCardDrawn) eventData;
        if (data.cardDrawn.AMBIENCE == CardAmbient.DIALOGUE){
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(data.cardDrawn.OWNER, new ArgumentFinesse(), 1));
        }
    }
}