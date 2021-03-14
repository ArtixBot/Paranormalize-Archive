using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAi : AbstractCharacter
{
    public PlayerAi(){
        this.NAME = "Ai";
        this.FACTION = FactionType.PLAYER;
        this.coreArgument = new ArgumentTalismanResolve();
        this.coreArgument.OWNER = this;
        this.maxAP = 3;
    }

    public override void AddStarterDeck(){
        //TODO: change from drawPile to battleDeck and implement deep-copy method
        // this.drawPile.AddCard("TINKER_ANCHOR_SLAM");
        // this.drawPile.AddCard("TINKER_WEIGHTED_HAMMER");
        // this.drawPile.AddCard("TINKER_FLASH_OF_BRILLIANCE");
        // this.drawPile.AddCard("TINKER_DEFECTIVE_IMPROVEMENTS");
        // this.drawPile.AddCard("TINKER_BLOCK");
    }
}