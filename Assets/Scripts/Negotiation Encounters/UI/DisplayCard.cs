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

    public Vector3 onHoverScaleAmount = new Vector3(0.1f, 0.1f, 0f);
    public Vector3 onHoverMoveAmount = new Vector3(0, 100, 0);
    public bool isInCardOverlay;        // is true when in the card selection (choose 0-X cards) overlay
    public bool isInDeckOverlay;        // is true when in the deck view overlay and for compendium view
    public bool isInRewardOverlay;      // is true when this is in the reward overlay
    public bool selectedInCardOverlay = false;  // should only be true whenever isInCardOverlay is true

    private Image cardRarityBorder;
    private Image cardBG;
    private Image cardImage;
    private Image cardInsigniaBG;
    private Image cardInsignia;
    private TextMeshProUGUI cardName;
    private TextMeshProUGUI cardCost;
    private TextMeshProUGUI cardType;
    private TextMeshProUGUI cardText;
    private GameObject keywordPrefab;
    private List<GameObject> keywordTooltips = new List<GameObject>();
    
    private int siblingIndex;
    private Quaternion origRotation;
    private Vector3 origScale;
    private Vector3 origPos;

    void Start(){
        keywordPrefab = Resources.Load("Prefabs/KeywordTooltip") as GameObject;
    }

    public void Render()
    {
        this.origScale = transform.localScale;
        this.origRotation = transform.rotation;
        this.origPos = transform.position;
        this.isInCardOverlay = transform.parent.name == "Card Display";         // default to false since cards are primarily in the hand only
        this.isInDeckOverlay = GameObject.Find("ViewDeckDisplay") != null;
        this.isInRewardOverlay = GameObject.Find("RewardViewOverlay") != null;

        cardRarityBorder = transform.Find("CardRarityBorder").GetComponent<Image>();
        cardBG = transform.Find("CardBG").GetComponent<Image>();
        cardImage = transform.Find("CardImage").GetComponent<Image>();
        cardInsigniaBG = transform.Find("CardInsigniaBG").GetComponent<Image>();
        cardInsignia = transform.Find("CardInsignia").GetComponent<Image>();
        cardName = transform.Find("CardName").GetComponent<TextMeshProUGUI>();
        cardCost = transform.Find("CardCost").GetComponent<TextMeshProUGUI>();
        cardType = transform.Find("CardType").GetComponent<TextMeshProUGUI>();
        cardText = transform.Find("CardDesc").GetComponent<TextMeshProUGUI>();
        if (reference == null){
            return;
        }
        cardImage.sprite = Resources.Load<Sprite>(reference.IMAGE);
        switch(reference.AMBIENCE){
            case CardAmbient.AGGRESSION:
                cardInsigniaBG.color = new Color32(74, 5, 5, 255);
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-aggression");
                break;
            case CardAmbient.DIALOGUE:
                cardInsigniaBG.color = new Color32(71, 94, 54, 255);
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-dialogue");
                break;
            case CardAmbient.INFLUENCE:
                cardInsigniaBG.color = new Color32(94, 54, 94, 255);
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-influence");
                break;
            case CardAmbient.PARANORMAL:
                cardInsigniaBG.color = new Color32(0, 125, 128, 255);
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-paranormal");
                break;
            case CardAmbient.STATUS:
                cardInsigniaBG.color = new Color32(94, 82, 54, 255);
                cardInsignia.sprite = Resources.Load<Sprite>("Images/insignia-status");
                break;
            default:
                break;
        }
        switch (reference.RARITY){
            case CardRarity.UNIQUE:
            case CardRarity.RARE:
                cardRarityBorder.color = new Color32(218, 165, 32, 255);
                break;
            case CardRarity.UNCOMMON:
                cardRarityBorder.color = new Color32(192, 192, 192, 255);
                break;
            case CardRarity.COMMON:
            case CardRarity.STARTER:
            default:
                cardRarityBorder.color = new Color32(50, 50, 50, 255);
                break;
        }
        cardName.text = reference.NAME.ToUpper();
        if (reference.isUpgraded){
            cardName.color = new Color32(169, 247, 101, 255);
        }
        cardCost.text = (reference.COSTS_ALL_AP) ? "X" : reference.COST.ToString();
        cardType.text = $"{reference.TYPE.ToString()}";
        cardText.text = ParseText(reference.DESC);
    }

    private string ParseText(string s){
        MatchCollection matches = new Regex(@"\[[^\]]*\]").Matches(s);
        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            
            try{
                if (match.Value == "[S]" || match.Value == "[s]"){                  // Argument stack parsing
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

    public void OnPointerClick(PointerEventData eventData){
        // on click, if the card is in the deck view overlay, is not upgraded, and a mastery point is available - upgrade it.
        if (isInDeckOverlay){
            if (!reference.isUpgraded && GameState.mastery > 0){
                GameObject deckOverlay = GameObject.Find("ViewDeckDisplay");
                GameState.mastery -= 1;
                reference.Upgrade();
                deckOverlay.SetActive(false);        // refresh deck overlay
                deckOverlay.SetActive(true);
            }
            return;
        }
        if (isInRewardOverlay){
            GameState.mainChar.AddCardToPermaDeck(this.reference.ID, this.reference.isUpgraded);
            RenderNegotiation renderer = GameObject.Find("RenderNegotiation").GetComponent<RenderNegotiation>();
            renderer.EndNegotiationRender();
            return;
        }
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
                cardBG.color = new Color32(50, 50, 50, 255);
            }
            instance.EnableButtonIfConditionsMet();
        }
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData){
        StartCoroutine(RunOnPointerEnter());
    }

    public void OnPointerExit(PointerEventData eventData){
        StartCoroutine(RunOnPointerExit());
    }

    float duration = 0.05f;
    IEnumerator RunOnPointerEnter(){
        if (this.keywordTooltips.Count == 0){
            for(int i = reference.TAGS.Count - 1; i >= 0; i--){
                CardTags tag = reference.TAGS[i];
                GameObject prefab = Instantiate(keywordPrefab, transform.position + new Vector3(300, 400 + (i * -100), 0), Quaternion.identity);
                this.keywordTooltips.Add(prefab);
                prefab.SetActive(false);
                prefab.GetComponent<TooltipKeyword>().title.text = tag.ToString().Substring(0, 1) + tag.ToString().Substring(1).ToLower();
                prefab.transform.SetParent(GameObject.Find("Canvas").transform);
                prefab.SetActive(true);
            }
        }
        StopCoroutine(RunOnPointerExit());
        if (!isInCardOverlay && !isInRewardOverlay){
            this.siblingIndex = transform.GetSiblingIndex();
            // transform.position = transform.position + new Vector3(0, 100, 0);
            transform.SetAsLastSibling();
        }

        float currentTime = 0f;
        Vector3 maxSize = this.origScale + this.onHoverScaleAmount;
        Vector3 maxMove = this.origPos + this.onHoverMoveAmount;
        while (currentTime <= duration){
            currentTime += Time.deltaTime;
            float normalized = Math.Min(currentTime / duration, 1.0f);

            Vector3 lerpSize = Vector3.Lerp(this.origScale, maxSize, normalized);
            transform.localScale = lerpSize;
            
            Quaternion lerpRotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, normalized);
            transform.rotation = lerpRotation;

            if (!isInCardOverlay && !isInRewardOverlay){
                Vector3 lerpMove = Vector3.Lerp(this.origPos, maxMove, normalized);
                transform.position = lerpMove;
            }
            yield return null;
        }
    }

    IEnumerator RunOnPointerExit(){
        foreach(GameObject tooltip in this.keywordTooltips){
            Destroy(tooltip);
        }
        this.keywordTooltips.Clear();

        StopCoroutine(RunOnPointerEnter());
        if (!isInCardOverlay && !isInRewardOverlay){
            // transform.position = transform.position + new Vector3(0, -100, 0);
            transform.SetSiblingIndex(siblingIndex);    
        }

        float currentTime = 0f;
        Quaternion curRot = transform.rotation;
        Vector3 curSize = transform.localScale;
        Vector3 curPos = transform.position;

        while (currentTime <= duration){
            currentTime += Time.deltaTime;
            float normalized = Math.Min(currentTime / duration, 1.0f);

            Vector3 lerpSize = Vector3.Lerp(curSize, this.origScale, normalized);
            transform.localScale = lerpSize;

            Quaternion lerpRotation = Quaternion.Lerp(curRot, this.origRotation, normalized);
            transform.rotation = lerpRotation;

            if (!isInCardOverlay && !isInRewardOverlay){
                Vector3 lerpPos = Vector3.Lerp(curPos, this.origPos, normalized);
                transform.position = lerpPos;
            }
            yield return null;
        }
    }
}
