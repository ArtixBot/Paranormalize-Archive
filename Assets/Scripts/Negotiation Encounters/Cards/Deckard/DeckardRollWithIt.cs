using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardRollWithIt : AbstractCard
{
    public static string cardID = "DECKARD_ROLL_WITH_IT";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public DeckardRollWithIt() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.UNCOMMON,
        CardType.SKILL
    ){
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){}

    public override void Upgrade(){
        base.Upgrade();
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventCardPlayed data = (EventCardPlayed) eventData;
        AbstractCard cardPlayed = data.cardPlayed;
        if (this.OWNER.GetHand().Contains(this) && data.cardPlayed.OWNER == this.OWNER){
            this.OWNER.GetHand().Remove(this);
            if (this.isUpgraded){
                this.OWNER.AddTempCardToHand(data.cardPlayed.ID, true);     // Upgrade card by default if this card is upgraded
            } else {
                this.OWNER.AddTempCardToHand(data.cardPlayed.ID, data.cardPlayed.isUpgraded);       // Otherwise, only upgrade card if the played card itself was upgraded
            }
        }
        return;
    }
}
