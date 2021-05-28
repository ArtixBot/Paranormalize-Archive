using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public interface ITriggerOnEvent{
    void TriggerOnEvent(AbstractEvent eventData);  
}

public class DisplayArgument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ITriggerOnEvent
{
    public AbstractArgument reference;

    public Image argumentImage;
    public TextMeshProUGUI stackCounter;
    public GameObject tooltipPrefab;
    public GameObject tooltipInstance;
    public Image healthBarImageFill;
    public TextMeshProUGUI healthBarText;


    public void OnEnable(){        
        tooltipPrefab = Resources.Load("Prefabs/ArgumentTooltip") as GameObject;
        argumentImage = transform.Find("Image").GetComponent<Image>();
        stackCounter = transform.Find("StackCount").GetComponent<TextMeshProUGUI>();

        healthBarImageFill = transform.Find("HealthBar/Fill").GetComponent<Image>();
        healthBarText = transform.Find("HealthBar/Text").GetComponent<TextMeshProUGUI>();

        argumentImage.sprite = reference.IMG;

        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_START);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.TURN_END);
        EventSystemManager.Instance.SubscribeToEvent(this, EventType.CARD_PLAYED);

        stackCounter.text = "x" + reference.stacks;
        healthBarImageFill.fillAmount =  (float)reference.curHP / (float)reference.maxHP;
        healthBarText.text = reference.curHP + "/" + reference.maxHP + " (" + reference.poise + ")";
        // Debug.Log("Enabled all things for " + reference.NAME);
    }

    public void OnDisable(){
        EventSystemManager.Instance.UnsubscribeFromEvent(this, EventType.TURN_START);
        EventSystemManager.Instance.UnsubscribeFromEvent(this, EventType.TURN_END);
        EventSystemManager.Instance.UnsubscribeFromEvent(this, EventType.CARD_PLAYED);
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if (tooltipInstance == null){
            int x = (reference.OWNER.FACTION == FactionType.PLAYER) ? 300 : -300;
            tooltipInstance = (GameObject) Instantiate(tooltipPrefab, transform.position + new Vector3(x, 0, 0), Quaternion.identity);
            tooltipInstance.SetActive(false);       // Workaround for now; force an OnEnable() call AFTER we load in data for the tooltip.
            tooltipInstance.GetComponent<TooltipArgument>().argRef = reference;
            tooltipInstance.transform.SetParent(GameObject.Find("Canvas").transform);
            tooltipInstance.SetActive(true);        // Forces an OnEnable() call so that the tooltip has the information (instead of nullref to AbstractArgument)
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        Destroy(tooltipInstance);
    }

    public void TriggerOnEvent(AbstractEvent eventData){
        stackCounter.text = "x" + reference.stacks;
        healthBarImageFill.fillAmount =  (float)reference.curHP / (float)reference.maxHP;
        healthBarText.text = reference.curHP + "/" + reference.maxHP + " (" + reference.poise + ")";
    }
}
