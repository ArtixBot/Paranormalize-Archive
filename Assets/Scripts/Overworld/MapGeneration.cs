using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterType {ENEMY = 0, ELITE = 1, SHOP = 2, FERRYMAN = 3, EVENT = 4, REST = 5, BOSS = 6};
public class MapGeneration{
    // Players go through 12 zones, but the second to last zone and last zone are always a rest point and boss, respectively.
    // Each zone can generate between 2-4 encounters to choose from.
    private EncounterType[][] map = new EncounterType[12][];

    public EncounterType[][] GenerateMap(){
        for (int i = 0; i < 10; i++){
            int choices = UnityEngine.Random.Range(2, 5);
            map[i] = new EncounterType[choices];
            for (int j = 0; j < choices; j++){
                // just generate a number between [0, 5] for now
                EncounterType encounter = (EncounterType)UnityEngine.Random.Range(0, (int)Enum.GetNames(typeof(EncounterType)).Length - 1);  // -1 to prevent random generation of boss encounters!
                map[i][j] = encounter;
            }            
        }
        map[10] = new EncounterType[1]{EncounterType.REST};
        map[11] = new EncounterType[1]{EncounterType.BOSS};

        // for (int i = 0; i < map.Length; i++){
        //     for (int j = 0; j < map[i].Length; j++){
        //         Debug.Log("Encounter at zone " + i + ": " + map[i][j]);
        //     }
        // }
        return map;
    }

}