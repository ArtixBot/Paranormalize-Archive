using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage yo turns. Singleton.
public class TurnManager
{
    public static readonly TurnManager Instance = new TurnManager();
    private List<AbstractCharacter> turnList = new List<AbstractCharacter>();

    public void AddToTurnList(AbstractCharacter character){
        turnList.Add(character);
    }

    public void NextCharacter(){
        // Debug.Log("TurnManager.cs: Ending turn for " + GetCurrentCharacter().NAME + ".");
        GetCurrentCharacter().EndTurn();        // Run end-of-turn function for current character.
        turnList.Add(GetCurrentCharacter());    // Add current character to the back of the queue.
        turnList.RemoveAt(0);                   // Remove them from the front (turn ended).
        GetCurrentCharacter().StartTurn();      // Run start-of-turn function for new character.
    }
    
    public AbstractCharacter GetCurrentCharacter(){
        return this.turnList[0];
    }

    public List<AbstractCharacter> GetTurnList(){
        return this.turnList;
    }

    public AbstractCharacter GetPlayer(){
        for (int i = 0; i < turnList.Count; i++){
            if (turnList[i].FACTION == FactionType.PLAYER){
                return turnList[i];
            }
        }
        return null;
    }

    public AbstractCharacter GetEnemy(){
        for (int i = 0; i < turnList.Count; i++){
            if (turnList[i].FACTION == FactionType.ENEMY){
                return turnList[i];
            }
        }
        return null;
    }

    // Return the character whose name does NOT equal that of returnOtherChar.
    // Called by TooltipArgument for [ENEMY] tag. Doesn't have much of a use otherwise.
    public AbstractCharacter GetOtherCharacter(AbstractCharacter returnOtherChar){
        for (int i = 0; i < turnList.Count; i++){
            if (turnList[i].NAME != returnOtherChar.NAME){
                return turnList[i];
            }
        }
        return null;
    }

    public void DebugTurnList(){
        foreach(var character in this.turnList){
            Debug.Log(character.NAME);
        }
    }
}