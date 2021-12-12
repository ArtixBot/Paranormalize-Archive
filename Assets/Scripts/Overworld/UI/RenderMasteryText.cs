using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RenderMasteryText : MonoBehaviour
{
    public TextMeshProUGUI masteryText;
    // Start is called before the first frame update
    void Awake(){
        masteryText = gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    // Update is called once per frame
    void Update(){
        masteryText.text = $"Mastery: {GameState.mastery}";
    }
}
