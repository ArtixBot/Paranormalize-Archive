using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAi : AbstractCharacter
{
    private static string playerID = "AI";
    private static string playerName = "Ai";
    private static AbstractArgument playerCoreArg = new ArgumentCoreAi();

    public PlayerAi(): base(playerID, playerName, playerCoreArg, true){}

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