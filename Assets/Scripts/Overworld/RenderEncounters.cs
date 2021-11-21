using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RenderEncounters : MonoBehaviour
{
    public Button optionA;
    public Button optionB;
    public Button optionC;
    public TextMeshProUGUI zoneCount;
    private EncounterType[][] map;
    // Start is called before the first frame update
    void Start(){
        MapGeneration mapGenerator = new MapGeneration();
        map = mapGenerator.GenerateMap();
    }

    // Update is called once per frame
    void Update(){
        zoneCount.text = "Zone " + (GameState.currentStage + 1);
    }
}
