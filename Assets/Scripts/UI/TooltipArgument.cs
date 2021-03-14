using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TooltipArgument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbstractArgument argRef;
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

        if (argRef != null){
            argumentType.GetComponent<TextMeshProUGUI>().text = (argRef.isCore) ? "Core Argument" : "Argument";
            argumentName.GetComponent<TextMeshProUGUI>().text = argRef.NAME;
            argumentDesc.GetComponent<TextMeshProUGUI>().text = ParseTooltip(argRef.DESC);
            argumentStacks.GetComponent<TextMeshProUGUI>().text = "x" + argRef.stacks;
            argumentResolve.GetComponent<TextMeshProUGUI>().text = "RESOLVE " + argRef.curHP + "/" + argRef.maxHP;
        } else {
            argumentName.GetComponent<TextMeshProUGUI>().text = "MISSING ARGUMENT REF";
        }
    }

    public string ParseTooltip(string s){
        MatchCollection matches = new Regex(@"\[[^\]]*\]").Matches(s);
        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            
            if (match.Value.Contains("STACKS")){
                Regex re = new Regex(@"\d+");
                Match stackMultiplier = re.Match(match.Value);
                if (stackMultiplier.Success){   // Handle [STACKS*X] and [STACKS/X]
                    int mult = int.Parse(stackMultiplier.Value);
                    s = match.Value.Contains("*") ? s.Replace(match.Value, (argRef.stacks * mult).ToString()) : s.Replace(match.Value, (argRef.stacks / mult).ToString()) ;
                } else {                        // Handle [STACKS]
                    s = s.Replace(match.Value, (argRef.stacks).ToString());
                }
            } else if (match.Value.Contains("ENEMY")){
                s = s.Replace(match.Value, TurnManager.Instance.GetEnemy().NAME);
            } else if (match.Value.Contains("OWNER")){
                s = s.Replace(match.Value, argRef.OWNER.NAME);
            } 
        }
        return s;
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}