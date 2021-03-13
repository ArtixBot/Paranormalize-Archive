using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipArgument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbstractArgument argumentRef;
    public Image bg;
    public GameObject argumentType;
    public GameObject argumentName;
    public GameObject argumentDesc;
    public GameObject argumentStacks;
    public GameObject argumentResolve;

    void OnEnable(){
        bg = transform.Find("BG").gameObject.GetComponent<Image>();
        argumentType = transform.Find("TitleBG/ArgumentType").gameObject;
        argumentName = transform.Find("TitleBG/Core Argument Name").gameObject;
        argumentResolve = transform.Find("TitleBG/ResolveValue").gameObject;
        argumentStacks = transform.Find("TitleBG/StackValue").gameObject;

        argumentDesc = transform.Find("BG/Core Argument Description").gameObject;

        if (argumentRef != null){
            argumentType.GetComponent<TextMeshProUGUI>().text = (argumentRef.isCore) ? "Core Argument" : "Argument";
            argumentName.GetComponent<TextMeshProUGUI>().text = argumentRef.NAME;
            argumentDesc.GetComponent<TextMeshProUGUI>().text = argumentRef.DESC;
            argumentStacks.GetComponent<TextMeshProUGUI>().text = "x" + argumentRef.stacks;
            argumentResolve.GetComponent<TextMeshProUGUI>().text = "RESOLVE " + argumentRef.curHP + "/" + argumentRef.maxHP;
        } else {
            argumentName.GetComponent<TextMeshProUGUI>().text = "MISSING ARGUMENT REF";
        }
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}