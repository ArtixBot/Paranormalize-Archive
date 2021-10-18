using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : AbstractEnemy{
    public TestEnemy(){
        this.NAME = "Test Dummy";
        this.FACTION = FactionType.ENEMY;
        this.coreArgument = new ArgumentCoreNoAbility();
        this.coreArgument.OWNER = this;
        this.maxAP = 3;

        this.AddStarterDeck();
    }

    public override void AddStarterDeck(){
        // this.AddCardToPermaDeck("DECKARD_DIPLOMACY");
        // this.AddCardToPermaDeck("DECKARD_DIPLOMACY");
        // this.AddCardToPermaDeck("DECKARD_GRUFF");
        // this.AddCardToPermaDeck("ENEMY_BACKLASH");
        // this.AddCardToPermaDeck("ENEMY_BACKLASH");
    }

    // Calculate a list of cards to play and what arguments to target w/ each card
    public override List<(AbstractCard, AbstractArgument)> CalculateCardsToPlay(){
        List<AbstractCard> currentHand = new List<AbstractCard>(this.GetHand());    // new keyword allows for "deep copy" since AbstractCard appears to be a primitive type... somehow
        List<(AbstractCard, AbstractArgument)> cardsToPlay = new List<(AbstractCard, AbstractArgument)>();

        int actionBudget = this.curAP;

        while (actionBudget > 0 && currentHand.Count > 0){      // literally just choose random cards
            int rand = Random.Range(0, currentHand.Count);
            if (currentHand[rand].COST <= actionBudget){
                cardsToPlay.Add( (currentHand[rand], TurnManager.Instance.GetPlayer().GetCoreArgument()) );     // just target core argument for now
                actionBudget -= currentHand[rand].COST;
                currentHand.RemoveAt(rand);
            }
        }
        return cardsToPlay;
    }
}