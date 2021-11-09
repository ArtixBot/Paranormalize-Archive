using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogBubble : MonoBehaviour
{
    public TextMeshProUGUI textContent;
    
    void OnEnable(){
        textContent = transform.Find("Dialog").GetComponent<TextMeshProUGUI>();
    }
}
