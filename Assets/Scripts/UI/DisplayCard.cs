using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DisplayCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AbstractCard reference;

    public Image cardBG;
    public Image cardImage;
    public Image cardInsignia;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardCost;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI cardText;
    public GameObject keywordPrefab;
    public List<GameObject> keywordTooltips = new List<GameObject>();

    public bool isInCardOverlay = false;
    public bool selectedInCardOverlay = false;  // should only be true whenever isInCardOverlay is true

    private int siblingIndex;
    private Quaternion origRotation;
    private Vector3 origScale;

    void Start(){
        keywordPrefab = Resources.Load("Prefabs/KeywordTooltip") as GameObject;
    }
    public void Render()
    {
        cardBG = transform.Find("CardBG").GetComponent<Image>();
        cardImage = transform.Find("CardImage").GetComponent<Image>();
        cardInsignia = transform.Find("CardInsignia").GetComponent<Image>();
        cardName = transform.Find("CardName").GetComponent<TextMeshProUGUI>();
        cardCost = transform.Find("CardCost").GetComponent<TextMeshProUGUI>();
        cardType = transform.Find("CardType").GetComponent<TextMeshProUGUI>();
        cardText = transform.Find("CardDesc").GetComponent<TextMeshProUGUI>();
        if (reference == null){
            return;
        }
        cardImage.sprite = reference.IMAGE;
        switch(reference.AMBIENCE){
            case CardAmbient.AGGRESSION:
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-aggression");
                break;
            case CardAmbient.DIALOGUE:
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-dialogue");
                break;
            case CardAmbient.INFLUENCE:
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-influence");
                break;
            default:
                break;
        }
        cardName.text = reference.NAME;
        cardCost.text = reference.COST.ToString();
        cardType.text = reference.TYPE.ToString();
        cardText.text = ParseText(reference.DESC);
    }

    private string ParseText(string s){
        MatchCollection matches = new Regex(@"\[[^\]]*\]").Matches(s);
        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            
            try{
                if (match.Value == "[S]"){                  // Argument stack parsing
                    int stacks = (int)reference.GetType().GetField("STACKS").GetValue(reference);
                    s = s.Replace(match.Value, stacks.ToString());
                } else if (match.Value == "[D]"){           // Damage parsing
                    int min = (int)reference.GetType().GetField("MIN_DAMAGE").GetValue(reference);
                    int max = (int)reference.GetType().GetField("MAX_DAMAGE").GetValue(reference);
                    if (min == max){
                        s = s.Replace(match.Value, min.ToString());
                    } else {
                        s = s.Replace(match.Value, min.ToString() + "-" + max.ToString());
                    }
                } else if (match.Value == "[P]"){           // Poise parsing
                    int poise = (int)reference.GetType().GetField("POISE").GetValue(reference);
                    s = s.Replace(match.Value, poise.ToString());
                } else if (match.Value == "[N]"){           // Draw parsing
                    int poise = (int)reference.GetType().GetField("DRAW").GetValue(reference);
                    s = s.Replace(match.Value, poise.ToString());
                }else {                                     // Custom parsing (e.g. if we do [ABC], searches for an 'ABC' field in the card)
                    string parsed = match.Value.Substring(1, match.Value.Length-2);     // Remove [ and ] characters
                    int customValue = (int)reference.GetType().GetField(parsed).GetValue(reference);
                    s = s.Replace(match.Value, customValue.ToString());
                }
            } catch{
                Debug.Log("Unable to parse " + match.Value + " for " + reference.NAME + ": check .json and .cs files to make sure right field names are used!");
                continue;
            }
        }
        return s;
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData){
        StartCoroutine(RunOnPointerEnter());
        if (this.keywordTooltips.Count == 0){
            for(int i = reference.TAGS.Count - 1; i >= 0; i--){
                // Debug.Log(reference.TAGS[i]);
                CardTags tag = reference.TAGS[i];
                GameObject prefab = Instantiate(keywordPrefab, transform.position + new Vector3(300, 400 + (i * -100), 0), Quaternion.identity);
                prefab.SetActive(false);
                prefab.GetComponent<TooltipKeyword>().title.text = tag.ToString().Substring(0, 1) + tag.ToString().Substring(1).ToLower();
                prefab.transform.SetParent(GameObject.Find("Canvas").transform);
                prefab.SetActive(true);
                this.keywordTooltips.Add(prefab);
                // Debug.Log(tag.ToString() + " " + LocalizationLibrary.Instance.GetKeywordString(tag.ToString()));
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        StartCoroutine(RunOnPointerExit());
        foreach(GameObject tooltip in this.keywordTooltips){
            Destroy(tooltip);
        }
        this.keywordTooltips.Clear();
    }

    public void OnPointerClick(PointerEventData eventData){
        if (isInCardOverlay){
            selectedInCardOverlay = !selectedInCardOverlay;
            // TODO: Probably change from GameObject.Find("SelectCardOverlay(Clone)") to instead grab parent directly
            // Grabbing parent directly should work since isInCardOverlay is checked
            SelectCardOverlay instance = GameObject.Find("SelectCardOverlay(Clone)").GetComponent<SelectCardOverlay>();

            if (selectedInCardOverlay){
                instance.selectedCards.Add(reference);
                cardBG.color = Color.green;
            } else {
                instance.selectedCards.Remove(reference);
                cardBG.color = new Color32(159, 159, 159, 255);
            }
            instance.EnableButtonIfConditionsMet();
        }
    }

    float duration = 0.015f;
    float currentTime = 0f;
    IEnumerator RunOnPointerEnter(){
        this.origScale = transform.localScale;
        float currentTime = 0f;
        float normalized = 0f;
        Vector3 increment = new Vector3(0.1f, 0.1f, 0);
        while (currentTime <= duration){
            currentTime += Time.deltaTime;
            normalized = Math.Min(currentTime / duration, 1.0f);
            transform.localScale = this.origScale + (increment * normalized);
            yield return null;
        }
        if (!isInCardOverlay){
            this.siblingIndex = transform.GetSiblingIndex();
            this.origRotation = transform.rotation;

            transform.rotation = Quaternion.identity;
            transform.position = transform.position + new Vector3(0, 100, 0);
            transform.SetAsLastSibling();
        }
    }

    IEnumerator RunOnPointerExit(){
        float currentTime = 0f;
        float normalized = 0f;
        Vector3 bigScale = this.origScale + new Vector3(0.1f, 0.1f, 0);
        Vector3 decrement = transform.localScale - this.origScale;
        while (currentTime <= duration){
            currentTime += Time.deltaTime;
            normalized = Math.Min(currentTime / duration, 1.0f);
            transform.localScale = bigScale + (decrement * normalized);
            yield return null;
        }
        transform.localScale -= new Vector3(0.1f, 0.1f, 0);
        if (!isInCardOverlay){
            transform.rotation = this.origRotation;
            transform.position = transform.position + new Vector3(0, -100, 0);
            transform.SetSiblingIndex(siblingIndex);    
        }
        yield return null;
    }
}
