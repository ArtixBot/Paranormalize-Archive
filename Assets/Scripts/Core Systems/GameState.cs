using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState{
    public static AbstractCharacter mainChar {get; set;}
    public static AbstractCharacter sidekick {get; set;}
    public static int currentStage {get; set;}
}
