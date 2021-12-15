using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RenderResolveText : MonoBehaviour
{
    public TextMeshProUGUI resolveText;
    // Start is called before the first frame update
    void Awake(){
        resolveText = gameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
    }

    // Update is called once per frame
    void Update(){
        resolveText.text = $"{GameState.mainChar.coreArgument.curHP}/{GameState.mainChar.coreArgument.maxHP}";
    }
}
