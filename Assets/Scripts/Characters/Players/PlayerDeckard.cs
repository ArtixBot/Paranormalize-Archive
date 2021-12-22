using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckard : AbstractCharacter
{
    private static string playerID = "DECKARD";
    private static string playerName = "Deckard";
    private static AbstractArgument playerCoreArg = new ArgumentCoreDeckard();

    public PlayerDeckard() : base(playerID, playerName, playerCoreArg, true){}

    public override void AddStarterDeck(){
        // this.AddCardToPermaDeck("DECKARD_COMEBACK");
        // this.AddCardToPermaDeck("DECKARD_DIPLOMACY");
        // this.AddCardToPermaDeck("DECKARD_BARREL_THROUGH");
        // this.AddCardToPermaDeck("DECKARD_INSTINCTS_FINESSE");
        // this.AddCardToPermaDeck("DECKARD_INSTINCTS_HEATED");
        // this.AddCardToPermaDeck("DECKARD_INSTINCTS");
        // this.AddCardToPermaDeck("DECKARD_REVERSAL");
        // this.AddCardToPermaDeck("DECKARD_BRASH");
        // this.AddCardToPermaDeck("DECKARD_GOOD_IMPRESSION");
        // this.AddCardToPermaDeck("DECKARD_GRANDIOSITY");
        // this.AddCardToPermaDeck("DECKARD_INSULT");
        // this.AddCardToPermaDeck("DECKARD_CALM");
        // this.AddCardToPermaDeck("DECKARD_RAPID_FIRE");
        // this.AddCardToPermaDeck("DECKARD_RAPID_FIRE");
        // this.AddCardToPermaDeck("DECKARD_BURST_OF_ANGER");
        // this.AddCardToPermaDeck("DECKARD_AUTHORITATIVE", true);
        // this.AddCardToPermaDeck("DECKARD_WORDSMITH");
        // this.AddCardToPermaDeck("DECKARD_PRESENT_THE_EVIDENCE");
        // this.AddCardToPermaDeck("DECKARD_STAY_COOL");
        // this.AddCardToPermaDeck("DECKARD_BULLDOZE");
        // this.AddCardToPermaDeck("DECKARD_PIVOT");
        // this.AddCardToPermaDeck("DECKARD_FLATTER");
        // this.AddCardToPermaDeck("DECKARD_STOIC");
        // this.AddCardToPermaDeck("DECKARD_CURVEBALL");
        // this.AddCardToPermaDeck("DECKARD_KNACK");
        // this.AddCardToPermaDeck("DECKARD_MAGNETIC_PERSONALITY");
        // this.AddCardToPermaDeck("DECKARD_FILIBUSTER");
        // this.AddCardToPermaDeck("DECKARD_REINFORCE");
        // this.AddCardToPermaDeck("DECKARD_COMMAND");
        // this.AddCardToPermaDeck("DECKARD_IMPATIENCE");
        // this.AddCardToPermaDeck("DECKARD_SIMMER");
        // this.AddCardToPermaDeck("DECKARD_DEFUSAL");
        // this.AddCardToPermaDeck("DECKARD_CROSS_EXAMINATION");
        // this.AddCardToPermaDeck("DECKARD_DEJA_VU", true);
        // this.AddCardToPermaDeck("DECKARD_FOLLOW_UP");
        // this.AddCardToPermaDeck("DECKARD_DECISIVE_ACTION");
        // this.AddCardToPermaDeck("DECKARD_OVERWHELM");
        // this.AddCardToPermaDeck("DECKARD_OPENING_OFFER");
        // this.AddCardToPermaDeck("DECKARD_ADAPTIVE");
        // this.AddCardToPermaDeck("DECKARD_RUMINATE");
        // this.AddCardToPermaDeck("DECKARD_DEEP_BREATH");
        // this.AddCardToPermaDeck("DECKARD_GUARDED_RESPONSE");
        // this.AddCardToPermaDeck("DECKARD_BREAKTHROUGH");
        // this.AddCardToPermaDeck("DECKARD_BREAKTHROUGH");
        // this.AddCardToPermaDeck("DECKARD_FOLLOW_UP");
        this.AddCardToPermaDeck("DECKARD_RUMINATE");
        this.AddCardToPermaDeck("DECKARD_DOMINEER");
        this.AddCardToPermaDeck("DECKARD_STRAWMAN");
        this.AddCardToPermaDeck("DECKARD_PRESENT_THE_EVIDENCE");
        // this.AddCardToPermaDeck("DECKARD_OVERBEAR");
        // this.AddCardToPermaDeck("DECKARD_RUMINATE");
        // this.AddCardToPermaDeck("DECKARD_BARRAGE");
        // this.AddCardToPermaDeck("DECKARD_INSIGHT");
        // this.AddCardToPermaDeck("DECKARD_CAPITALIZE");
        // this.AddCardToPermaDeck("DECKARD_OBLIGATION");
        // this.AddCardToPermaDeck("DECKARD_OBLIGATION", true);
        // this.AddCardToPermaDeck("DECKARD_PANACHE");
        this.AddCardToPermaDeck("DECKARD_SEETHE");
        // this.AddCardToPermaDeck("DECKARD_SEETHE", true);
        // this.AddCardToPermaDeck("DECKARD_INSTINCTS");
        // this.AddCardToPermaDeck("DECKARD_INSTINCTS", true);
        // this.AddCardToPermaDeck("DECKARD_RACK_THE_BRAIN");
    }
}