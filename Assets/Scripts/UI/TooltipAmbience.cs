using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipAmbience : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Ambience ambRef = NegotiationManager.Instance.ambience;
    public TextMeshProUGUI ambienceState;
    public TextMeshProUGUI ambienceDesc;
    public TextMeshProUGUI ambienceEffects;

    void OnEnable(){
        ambienceState = transform.Find("TitleBG/Current Ambience").GetComponent<TextMeshProUGUI>();
        ambienceDesc = transform.Find("TitleBG/Ambience Descriptor").GetComponent<TextMeshProUGUI>();
        ambienceEffects = transform.Find("BG/Ambience Effects").GetComponent<TextMeshProUGUI>();

        // TODO: This stuff should be read from a .json file.
        ambienceState.text = ambRef.GetState().ToString();
        ambienceDesc.text = "Paranormalization holding steady. Nothing too dicey, for now.";
        ambienceEffects.text = "- No special effects.\n- Play X|Y Dialogue|Aggression cards to change Ambience.";
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}