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
        // this.AddCardToPermaDeck("DECKARD_DIPLOMACY");
        // this.AddCardToPermaDeck("DECKARD_BARREL_THROUGH");
        this.AddCardToPermaDeck("DECKARD_INSTINCTS_HEATED");
        // this.AddCardToPermaDeck("DECKARD_GOOD_IMPRESSION");
        // this.AddCardToPermaDeck("DECKARD_GRUFF");
        // this.AddCardToPermaDeck("DECKARD_CALM");
        // this.AddCardToPermaDeck("DECKARD_SNIPING_REMARK");
        // this.AddCardToPermaDeck("DECKARD_HOTHEADED");
        // this.AddCardToPermaDeck("DECKARD_STAY_COOL");
        this.AddCardToPermaDeck("DECKARD_TRASH_TALK");
        // this.AddCardToPermaDeck("DECKARD_STOIC");
        // this.AddCardToPermaDeck("DECKARD_INTERVENE");
        // this.AddCardToPermaDeck("DECKARD_CHALLENGE");
        // this.AddCardToPermaDeck("DECKARD_JABBER");
        // this.AddCardToPermaDeck("DECKARD_FOLLOW_UP");
        // this.AddCardToPermaDeck("DECKARD_ADDENDUM");
        // this.AddCardToPermaDeck("DECKARD_BREAKTHROUGH");
        // this.AddCardToPermaDeck("DECKARD_OPENING_OFFER");
        // this.AddCardToPermaDeck("DECKARD_ADAPTIVE");
        // this.AddCardToPermaDeck("DECKARD_RUMINATE");
        // this.AddCardToPermaDeck("DECKARD_DEEP_BREATH");
        // this.AddCardToPermaDeck("DECKARD_GUARDED_RESPONSE");
        // this.AddCardToPermaDeck("DECKARD_BREAKTHROUGH");
        // this.AddCardToPermaDeck("DECKARD_BREAKTHROUGH");
        // this.AddCardToPermaDeck("DECKARD_FOLLOW_UP");
        // this.AddCardToPermaDeck("DECKARD_RUMINATE");
        // this.AddCardToPermaDeck("DECKARD_RUMINATE");
        // this.AddCardToPermaDeck("DECKARD_RUMINATE");
    }
}