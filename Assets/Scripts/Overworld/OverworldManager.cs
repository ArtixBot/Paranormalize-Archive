using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterType {ENEMY = 0, ELITE = 1, SHOP = 2, REST = 3, EVENT = 4, FERRYMAN = 5, BOSS = 6};
public enum EncounterModifier {INCREASED_TRAVEL_TIME, ENEMY_INCREASED_RESOLVE, ENEMY_REDUCED_RESOLVE}

public class EncounterInfo {

    // Metadata
    public EncounterType encounterType;
    public string storyID;                                  // story ID to use if a story encounter
    public string enemyID;                                  // enemy to use if an enemy encounter

    // Rendering
    public int renderPosX;              // Where to render this on the overworld. Set by RenderOverworld if null.
    public int renderPosY;              // See above.

    // Gameplay
    public int encounterLifetime;       // How long this encounter will last before it gets removed from the pool.
    public int timeCost;                // How many hours this encounter will consume. 
    List<EncounterModifier> encounterMods = new List<EncounterModifier>();
    
    public EncounterInfo(EncounterType encounterType, int lifetime = 6, int cost = 2){
        this.encounterType = encounterType;
        this.encounterLifetime = lifetime;
        this.timeCost = cost;
    }
}

public class OverworldManager{
    public static readonly OverworldManager Instance = new OverworldManager();
    // Players have 24 hours in each zone (can be modified by events) before the boss encounter spawns, after which all other active encounters are removed.

    // Encounters:
    // Traveling to an encounter takes 2 hours, giving players a baseline of 12 encounters per stage.
    // By default an encounter lasts for 6 hours so if a player does not select that node after 3 selections, the encounter disappears.
    // Can have modifiers; e.g. traveling to this encounter costs additional hours, negotiation is easier/more difficult/has different effects...
    public List<EncounterInfo> activeEncounters = new List<EncounterInfo>();
    public EncounterInfo lastCompletedEncounter;

    // Call this every time after an encounter is completed
    public void ManageOverworld(){
        HandleExistingEncounters();
        GenerateNewEncounters();
    }

    public void EncounterSelected(EncounterInfo encounter){
        this.activeEncounters.Remove(encounter);
        GameState.hoursRemainingInCurrentStage -= encounter.timeCost;
    }

    private void HandleExistingEncounters(){
        foreach(EncounterInfo encounter in activeEncounters){
            encounter.encounterLifetime -= 2;       // TODO: Change based on how long it took to complete the last encounter instead of a passive -2.
        }
        activeEncounters.RemoveAll(encounter => encounter.encounterLifetime <= 0);
    }

    private void GenerateNewEncounters(){
        if (GameState.hoursRemainingInCurrentStage == 24){                  // Force a basic enemy encounter as the first one.
            EncounterInfo initialEncounter = new EncounterInfo(EncounterType.ENEMY);
            initialEncounter.enemyID = "TEST_DUMMY";
            activeEncounters.Add(initialEncounter);
            return;
        }
        if (GameState.hoursRemainingInCurrentStage <= 0){                   // Boss time!
            activeEncounters.Clear();
            EncounterInfo bossEncounter = new EncounterInfo(EncounterType.BOSS);
            activeEncounters.Add(bossEncounter);
            return;
        }
        if (activeEncounters.Count >= 5){           // Don't generate any more if too many are still sitting there
            return;
        }

        // Always generate at least 1 enemy encounter and then randomize the other generated encounters (excluding Ferryman and Boss)
        EncounterInfo guaranteedEnemyEncounter = new EncounterInfo(EncounterType.ENEMY);
        guaranteedEnemyEncounter.enemyID = "TEST_DUMMY";
        activeEncounters.Add(guaranteedEnemyEncounter);

        int encountersToCreate = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < encountersToCreate; i++){
            EncounterType type = (EncounterType)UnityEngine.Random.Range(0, 5);
            EncounterInfo newEncounter = new EncounterInfo(type);

            if (type == EncounterType.ENEMY || type == EncounterType.ELITE){
                newEncounter.enemyID = "TEST_DUMMY";
            } else {
                newEncounter.storyID = "REST_STORY";
            }
            activeEncounters.Add(newEncounter);
        }
        return;
    }

}