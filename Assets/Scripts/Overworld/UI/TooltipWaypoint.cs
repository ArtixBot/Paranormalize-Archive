using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipWaypoint : MonoBehaviour
{
    public EncounterInfo reference;
    public TextMeshProUGUI travelDuration;
    public TextMeshProUGUI remainingDuration;

    void OnEnable(){
        travelDuration = transform.Find("StaticTooltipComponent/TravelBG/Text").GetComponent<TextMeshProUGUI>();
        remainingDuration = transform.Find("StaticTooltipComponent/LifetimeBG/Text").GetComponent<TextMeshProUGUI>();
    }

    public void SetText(){
        travelDuration.text = ParseTooltip(LocalizationLibrary.Instance.GetUIString("TOOLTIP_WAYPOINT_TRAVEL_TIME"));
        remainingDuration.text = (reference.encounterLifetime <= 2) ? ParseTooltip(LocalizationLibrary.Instance.GetUIString("TOOLTIP_WAYPOINT_EXPIRE_TIME_CRITICAL")) : ParseTooltip(LocalizationLibrary.Instance.GetUIString("TOOLTIP_WAYPOINT_EXPIRE_TIME"));
    }

    string ParseTooltip(string s){
        MatchCollection matches = new Regex(@"\[[^\]]*\]").Matches(s);
        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            
            if (match.Value.Contains("HOURS_TRAVEL")){
                s = s.Replace(match.Value, reference.timeCost.ToString());
            } else if (match.Value.Contains("HOURS_EXPIRE")){
                s = s.Replace(match.Value, reference.encounterLifetime.ToString());
            }
        }
        return s;
    }
}