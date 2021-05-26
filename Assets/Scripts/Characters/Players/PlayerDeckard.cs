using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckard : AbstractCharacter
{
    public PlayerDeckard(){
        this.NAME = "Deckard";
        this.FACTION = FactionType.PLAYER;
        this.coreArgument = new ArgumentCoreDeckard();
        this.coreArgument.OWNER = this;
        this.maxAP = 3;

        this.AddStarterDeck();
    }

    public override void AddStarterDeck(){
        this.permaDeck.AddCard("DECKARD_DIPLOMACY");
        this.permaDeck.AddCard("DECKARD_BARREL_THROUGH");
        this.permaDeck.AddCard("DECKARD_BRASH");
        this.permaDeck.AddCard("DECKARD_GOOD_IMPRESSION");
        this.permaDeck.AddCard("DECKARD_GRUFF");
        this.permaDeck.AddCard("DECKARD_CALM");
        this.permaDeck.AddCard("DECKARD_HOTHEADED");
        this.permaDeck.AddCard("DECKARD_SNIPING_REMARK");
        this.permaDeck.AddCard("DECKARD_STAY_COOL");
        this.permaDeck.AddCard("DECKARD_TRASH_TALK");
        this.permaDeck.AddCard("DECKARD_STOIC");
        this.permaDeck.AddCard("DECKARD_INTERVENE");
        this.permaDeck.AddCard("DECKARD_CHALLENGE");
        this.permaDeck.AddCard("DECKARD_JABBER");
        this.permaDeck.AddCard("DECKARD_FOLLOW_UP");
        this.permaDeck.AddCard("DECKARD_ADDENDUM");
        this.permaDeck.AddCard("DECKARD_DEEP_BREATH");
        this.permaDeck.AddCard("DECKARD_GUARDED_RESPONSE");
    }
}