using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DisplayCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbstractCard reference;

    public Sprite cardImage;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardCost;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI cardText;

    // Start is called before the first frame update
    void OnEnable()
    {
        cardImage = transform.Find("CardImage").GetComponent<Sprite>();
        cardName = transform.Find("CardName").GetComponent<TextMeshProUGUI>();
        cardCost = transform.Find("CardCost").GetComponent<TextMeshProUGUI>();
        cardType = transform.Find("CardType").GetComponent<TextMeshProUGUI>();
        cardText = transform.Find("CardDesc").GetComponent<TextMeshProUGUI>();
        if (reference == null){
            return;
        }
        cardImage = reference.IMAGE;
        cardName.text = reference.NAME;
        cardCost.text = reference.COST.ToString();
        cardType.text = reference.TYPE.ToString();
        cardText.text = reference.DESC;
    }

    public void RefreshText(){
        if (reference == null){
            return;
        }
        cardCost.text = reference.COST.ToString();
        cardText.text = reference.DESC.ToString();      // TODO: Handle [] params in the description
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData){
        transform.localScale += new Vector3(0.1f, 0.1f, 0);
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.localScale -= new Vector3(0.1f, 0.1f, 0);
        transform.SetSiblingIndex(0);    
    }
}
