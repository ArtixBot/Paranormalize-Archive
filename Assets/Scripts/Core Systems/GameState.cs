using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState{
    public static AbstractCharacter mainChar = new PlayerDeckard();     // TODO: this is just for testing purposes, reset to {get; set;} at some point
    public static AbstractCharacter ally {get; set;}

    // public static List<AbstractCard> commonDraftPool {get; set;}
    // public static List<AbstractCard> uncommonDraftPool {get; set;}
    // public static List<AbstractCard> rareDraftPool {get; set;}

    // Metadata
    public static string currentStoryID {get; set;}
    public static int currentStage {get; set;}
    public static int difficultyLevel {get; set;}
    public static int rngSeed {get; set;}
    public static int cardsAddedThisGame {get; set;}

    public static int mastery {get; set;}
    public static int money {get; set;}
    
    public static float moneyGainMod = 1.0f;
}
