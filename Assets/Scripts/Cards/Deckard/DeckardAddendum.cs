using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // target a random enemy argument
        if (data.cardDrawn == this){
            Debug.Log("Addendum triggers!");
            List<AbstractArgument> targets = TurnManager.Instance.GetOtherCharacter(data.owner).GetArguments();
            targets.Add(TurnManager.Instance.GetOtherCharacter(data.owner).GetCoreArgument());
            var random = new System.Random();
            int index = random.Next(targets.Count);

            NegotiationManager.Instance.AddAction(new DamageAction(targets[index], MIN_DAMAGE, MAX_DAMAGE));
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(data.owner, new ArgumentStrawman(), STACKS)); //TODO: Change to Finesse argument
        }
    }
}