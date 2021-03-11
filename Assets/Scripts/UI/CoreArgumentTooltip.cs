using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CoreArgumentTooltip : MonoBehaviour
{

    public AbstractArgument argumentRef;
    public Image bg;
    private TextMeshProUGUI argumentName;
    private TextMeshProUGUI argumentDesc;

    void OnEnable(){
        argumentRef = new CoreDeckard();
        bg = transform.Find("BG").gameObject.GetComponent<Image>();
        argumentName = transform.Find("Core Argument Name").gameObject.GetComponent<TextMeshProUGUI>();
        argumentDesc = transform.Find("Core Argument Description").gameObject.GetComponent<TextMeshProUGUI>();

        if (argumentRef != null){
            argumentName.text = argumentRef.NAME;
            argumentDesc.text = argumentRef.DESC;
        } else {
            argumentName.text = "MISSING ARGUMENT REF";
        }
    }
}