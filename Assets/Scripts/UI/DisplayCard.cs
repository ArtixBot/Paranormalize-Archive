using System.Collections;
using System.Collections.Generic;
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

    public bool isInCardOverlay = false;
    public bool selectedInCardOverlay = false;  // should only be true whenever isInCardOverlay is true

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
        cardText.text = reference.DESC;
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData){
        transform.localScale += new Vector3(0.1f, 0.1f, 0);
        // transform.position = transform.position + new Vector3(0, 0, 1);
        // transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.localScale -= new Vector3(0.1f, 0.1f, 0);
        // transform.position = transform.position + new Vector3(0, 0, -1);
        // transform.SetSiblingIndex(0);    
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
}
