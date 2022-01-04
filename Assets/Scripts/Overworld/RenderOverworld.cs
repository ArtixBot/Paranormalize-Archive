using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Renders the overworld screen. Also invokes NegotiationManager's StartNegotiation() to set up game state.
// Maybe have it also handle user input???
public class RenderOverworld : MonoBehaviour{
    public static OverworldManager om = OverworldManager.Instance;
    public GameObject waypointPrefab;
    public List<GameObject> prefabList = new List<GameObject>();

    void Start(){
        waypointPrefab = Resources.Load<GameObject>("Prefabs/OverworldWaypoint");
        
        foreach(GameObject obj in prefabList){
            Destroy(obj);
        }

        om.ManageOverworld();
        
        Debug.Log($"Overworld was managed; currently {om.activeEncounters.Count} active encounters.");
        for (int i = 0; i < om.activeEncounters.Count; i++){
            if (om.activeEncounters[i].renderPosX == default || om.activeEncounters[i].renderPosY == default){
                om.activeEncounters[i].renderPosX = UnityEngine.Random.Range(-200, 800);
                om.activeEncounters[i].renderPosY = UnityEngine.Random.Range(-500, 500);
            }
            GameObject newObj = (GameObject)Instantiate(waypointPrefab, this.transform);
            prefabList.Add(newObj);
            
            newObj.transform.SetParent(this.transform.Find("SpawnWaypoints"));
            newObj.transform.position = newObj.transform.position + new Vector3(om.activeEncounters[i].renderPosX, om.activeEncounters[i].renderPosY, 0);
            newObj.GetComponent<ButtonClickSceneChange>().encounterInfo = om.activeEncounters[i];
            newObj.GetComponent<ButtonClickSceneChange>().Render();
        }
    }
}