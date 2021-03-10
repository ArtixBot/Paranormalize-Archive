using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckard : AbstractCharacter
{
    public PlayerDeckard(){
        this.NAME = "Deckard";
        this.FACTION = FactionType.PLAYER;
        this.coreArgument = new CoreDeckard();
        this.maxAP = 3;
    }

    public override void AddStarterDeck(){
        //TODO: change from drawPile to permaDeck and implement deep-copy method
        this.drawPile.AddCard("DECKARD_DIPLOMACY");
    }
}