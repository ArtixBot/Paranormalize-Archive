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
        this.AddCardToPermaDeck("AI_BOORISH");
        this.AddCardToPermaDeck("AI_BOORISH");
        this.AddCardToPermaDeck("AI_TALISMAN_PURIFICATION");
        this.AddCardToPermaDeck("AI_RITES");
        this.AddCardToPermaDeck("AI_CHAT");
        this.AddCardToPermaDeck("AI_CHAT");
        this.AddCardToPermaDeck("AI_OVERLOOK");
        this.AddCardToPermaDeck("AI_OVERLOOK");
        this.AddCardToPermaDeck("AI_OVERLOOK");
        this.AddCardToPermaDeck("AI_OVERLOOK");
    }
}