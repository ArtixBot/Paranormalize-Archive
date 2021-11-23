using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState{
    public static AbstractCharacter mainChar = new PlayerDeckard();     // TODO: this is just for testing purposes, reset to {get; set;} at some point
    public static AbstractCharacter ally {get; set;}
    public static string currentStoryID {get; set;}
    public static int currentStage {get; set;}
    public static int money {get; set;}
    public static int randomSeed {get; set;}
}
