using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipCoreArgument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public AbstractArgument argumentRef;
    public Image bg;
    public GameObject argumentName;
    public GameObject argumentDesc;

    void OnEnable(){
        bg = transform.Find("BG").gameObject.GetComponent<Image>();
        argumentName = transform.Find("Core Argument Name").gameObject;
        argumentDesc = transform.Find("BG/Core Argument Description").gameObject;

        if (argumentRef != null){
            argumentName.GetComponent<TextMeshProUGUI>().text = argumentRef.NAME;
            argumentDesc.GetComponent<TextMeshProUGUI>().text = argumentRef.DESC;
        } else {
            argumentName.GetComponent<TextMeshProUGUI>().text = "MISSING ARGUMENT REF";
        }
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}