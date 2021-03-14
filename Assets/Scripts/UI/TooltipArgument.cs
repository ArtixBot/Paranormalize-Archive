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

    // Takes in a string and replaces certain keywords enclosed within [] with values.
    // Specifically, parses [STACKS], [STACKS*1], [STACKS/1] (any number can be used), [ENEMY], [OWNER].
    public string ParseTooltip(string s){
        MatchCollection matches = new Regex(@"\[[^\]]*\]").Matches(s);
        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            
            if (match.Value.Contains("STACKS")){
                Regex re = new Regex(@"\d+");
                Match stackMultiplier = re.Match(match.Value);
                if (stackMultiplier.Success){   // Handle [STACKS*X] and [STACKS/X]. [STACKS*3], for example, will convert 1 stack to a value of 3.
                    int mult = int.Parse(stackMultiplier.Value);
                    s = match.Value.Contains("*") ? s.Replace(match.Value, (argRef.stacks * mult).ToString()) : s.Replace(match.Value, (argRef.stacks / mult).ToString()) ;
                } else {                        // Handle [STACKS] at a 1-to-1 ratio, so 4 stacks of an argument = a value of 4
                    s = s.Replace(match.Value, (argRef.stacks).ToString());
                }
            } else if (match.Value.Contains("ENEMY")){  // Replace [ENEMY] with the name of the NON-OWNER for this argument.
                s = s.Replace(match.Value, TurnManager.Instance.GetEnemy().NAME);
            } else if (match.Value.Contains("OWNER")){  // Replace [OWNER] with the name of the OWNER for this argument.
                s = s.Replace(match.Value, argRef.OWNER.NAME);
            } 
        }
        return s;
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}