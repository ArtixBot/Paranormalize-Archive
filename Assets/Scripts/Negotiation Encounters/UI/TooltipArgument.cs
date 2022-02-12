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
            argumentDesc.GetComponent<TextMeshProUGUI>().text = ParseTooltip(argRef.DESC) + AddStatusEffectText();
            argumentStacks.GetComponent<TextMeshProUGUI>().text = "x" + argRef.stacks;
            if (argRef.isTrait){
                argumentResolve.GetComponent<TextMeshProUGUI>().text = "TRAIT";
            } else {
                argumentResolve.GetComponent<TextMeshProUGUI>().text = "RESOLVE " + argRef.curHP + "/" + argRef.maxHP + " (" + argRef.poise + ")";
            }
        } else {
            argumentName.GetComponent<TextMeshProUGUI>().text = "MISSING ARGUMENT REF";
        }
    }

    // Takes in a string and replaces certain keywords enclosed within [] with values.
    // Specifically, parses [STACKS], [STACKS*1], [STACKS/1] (any number can be used), [ENEMY], [OWNER].
    string ParseTooltip(string s){
        MatchCollection matches = new Regex(@"\[[^\]]*\]").Matches(s);
        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            
            // Handle [STACKS], [STACKS*X] and [STACKS/X]. [STACKS*3], for example, will convert 1 stack to a value of 3.
            if (match.Value.Contains("STACKS")){
                Regex re = new Regex(@"\d+");
                string prefix = "<style=\"Scalable\">";
                string suffix = "</style>";
                Match stackMultiplier = re.Match(match.Value);
                if (stackMultiplier.Success){   
                    int mult = int.Parse(stackMultiplier.Value);
                    s = match.Value.Contains("*") ? s.Replace(match.Value, prefix + (argRef.stacks * mult).ToString() + suffix) : s.Replace(match.Value, prefix + (argRef.stacks / mult).ToString() + suffix) ;
                } else {
                    s = s.Replace(match.Value, prefix + (argRef.stacks).ToString() + suffix);
                }
            }
            // Handle [ENEMY] with the name of the NON-OWNER for this argument.
            else if (match.Value.Contains("ENEMY")){
                s = s.Replace(match.Value, TurnManager.Instance.GetOtherCharacter(argRef.OWNER).NAME);
            }
            // Replace [OWNER] with the name of the OWNER for this argument.
            else if (match.Value.Contains("OWNER")){
                s = s.Replace(match.Value, argRef.OWNER.NAME);
            } 
        }
        return s;
    }

    // If the argument has any status effects, also add the description of that effect into here
    string AddStatusEffectText(){
        string retVal = "";
        foreach(AbstractStatusEffect effect in argRef.statusEffects){
            string color = (effect.TYPE == StatusEffectType.BUFF) ? "#a3ff8a" : "#ff7369";
            string sprite = (effect.TYPE == StatusEffectType.BUFF) ? "<sprite=\"icon-buff\" index=0>" : "<sprite=\"icon-debuff\" index=0>";
            retVal += $"\n<color={color}>{sprite}{effect.NAME}</color> - {ParseTooltip(effect.DESC)}\n";
        }
        if (retVal != ""){
            retVal = "\n\n<b>Status Effects</b>" + retVal;
        }
        return retVal;
    }

    public void OnPointerEnter(PointerEventData eventData){}

    public void OnPointerExit(PointerEventData eventData){}
}