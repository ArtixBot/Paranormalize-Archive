using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : AbstractCharacter
{
    public TestEnemy(){
        this.NAME = "Test Dummy";
        this.FACTION = FactionType.ENEMY;
        this.coreArgument = new ArgumentCoreVolatile();
        this.coreArgument.OWNER = this;
        this.maxAP = 3;

        this.AddStarterDeck();
    }

    public override void AddStarterDeck(){
        // this.drawPile.AddCard("TINKER_ANCHOR_SLAM");
        // this.drawPile.AddCard("TINKER_WEIGHTED_HAMMER");
        // this.drawPile.AddCard("TINKER_FLASH_OF_BRILLIANCE");
        // this.drawPile.AddCard("TINKER_DEFECTIVE_IMPROVEMENTS");
        // this.drawPile.AddCard("TINKER_BLOCK");
    }
}