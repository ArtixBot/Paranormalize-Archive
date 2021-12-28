using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RenderHourText : MonoBehaviour {
    public TextMeshProUGUI hourCount;
    // Start is called before the first frame update
    void Awake(){
        hourCount = gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    // Update is called once per frame
    void Update(){
        hourCount.text = $"BOSS ARRIVES IN <color=#00bbff>{GameState.hoursRemainingInCurrentStage}</color> HOURS";
    }
}
