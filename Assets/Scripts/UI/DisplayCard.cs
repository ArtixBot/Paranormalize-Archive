using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DisplayCard : MonoBehaviour
{
    public AbstractCard reference;

    public GameObject cardImage;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardCost;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI cardText;

    // Start is called before the first frame update
    void OnEnable()
    {
        cardImage = transform.Find("CardImage").gameObject;
        cardName = transform.Find("CardName").GetComponent<TextMeshProUGUI>();
        cardCost = transform.Find("CardCost").GetComponent<TextMeshProUGUI>();
        cardType = transform.Find("CardType").GetComponent<TextMeshProUGUI>();
        cardText = transform.Find("CardDesc").GetComponent<TextMeshProUGUI>();
    }

    void RefreshText(){
        if (reference == null){
            return;
        }
        cardCost.text = reference.COST.ToString();
        cardText.text = reference.DESC.ToString();      // TODO: Handle [] params in the description
    }
}
