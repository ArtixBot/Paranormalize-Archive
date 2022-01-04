using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : AbstractEnemy {
    
    private static string charID = "TEST_DUMMY";
    private static string charName = "Forlorn Spirit";
    private static AbstractArgument charCoreArg = new ArgumentCoreNoAbility();

    public TestEnemy() : base(charID, charName, charCoreArg, false){}

    public override void AddStarterDeck(){
        this.AddCardToPermaDeck("ENEMY_BACKLASH");
        this.AddCardToPermaDeck("ENEMY_BACKLASH");
        this.AddCardToPermaDeck("ENEMY_BACKLASH");
        this.AddCardToPermaDeck("ENEMY_NIHILISM");
        this.AddCardToPermaDeck("ENEMY_NIHILISM");
        this.AddCardToPermaDeck("ENEMY_NIHILISM");
        this.AddCardToPermaDeck("ENEMY_INSIDIOUS_WHISPERS");
        this.AddCardToPermaDeck("ENEMY_INSIDIOUS_WHISPERS");
        this.AddCardToPermaDeck("ENEMY_REGRETS");
        this.AddCardToPermaDeck("ENEMY_REGRETS");
    }
}