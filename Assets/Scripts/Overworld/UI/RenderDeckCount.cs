using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RenderDeckCount : MonoBehaviour
{
    public TextMeshProUGUI deckCount;
    // Start is called before the first frame update
    void Awake(){
        deckCount = gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    // Update is called once per frame
    void Update(){
        deckCount.text = $"{GameState.mainChar.permaDeck.GetSize()}";
    }
}
