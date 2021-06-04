using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipKeyword : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;

    void OnEnable(){
        title = transform.Find("TitleBG/Keyword").GetComponent<TextMeshProUGUI>();
        desc = transform.Find("BG/Description").GetComponent<TextMeshProUGUI>();

        desc.text = LocalizationLibrary.Instance.GetKeywordString(title.text.ToString().ToUpper());
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}