using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage yo turns. Singleton.
public class TurnManager
{
    public static readonly TurnManager Instance = new TurnManager();
    private List<AbstractCharacter> turnList = new List<AbstractCharacter>();

    private TurnManager(){
        // TODO: Remove this (currently for testing purposes ONLY)
        this.AddToTurnList(new PlayerAi());
        this.AddToTurnList(new TestEnemy());
    }

    public void AddToTurnList(AbstractCharacter character){
        turnList.Add(character);
    }

    public void NextCharacter(){
        // GetCurrentCharacter().EndTurn();        // Run end-of-turn function for current character.
        turnList.Add(GetCurrentCharacter());    // Add current character to the back of the queue.
        turnList.RemoveAt(0);                   // Remove them from the front (turn ended).
        // GetCurrentCharacter().StartTurn();      // Run start-of-turn function for new character.
    }
    
    public AbstractCharacter GetCurrentCharacter(){
        return this.turnList[0];
    }

    public List<AbstractCharacter> GetTurnList(){
        return this.turnList;
    }

    public void DebugTurnList(){
        foreach(var character in this.turnList){
            Debug.Log(character.NAME);
        }
    }
}