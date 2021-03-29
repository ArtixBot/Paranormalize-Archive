using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAi : AbstractCharacter
{
    public PlayerAi(){
        this.NAME = "Ai";
        this.FACTION = FactionType.PLAYER;
        this.coreArgument = new ArgumentCoreAi();
        this.coreArgument.OWNER = this;
        this.maxAP = 3;
        
        this.AddStarterDeck();
    }

    public override void AddStarterDeck(){
        this.permaDeck.AddCard("AI_BOORISH");
        this.permaDeck.AddCard("AI_BOORISH");
        this.permaDeck.AddCard("AI_BOORISH");
        this.permaDeck.AddCard("AI_RITES");
        this.permaDeck.AddCard("AI_CHAT");
        this.permaDeck.AddCard("AI_CHAT");
        this.permaDeck.AddCard("AI_OVERLOOK");
        this.permaDeck.AddCard("AI_OVERLOOK");
        this.permaDeck.AddCard("AI_OVERLOOK");
        this.permaDeck.AddCard("AI_OVERLOOK");
    }
}