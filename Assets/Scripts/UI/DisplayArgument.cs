using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DisplayArgument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbstractArgument reference;
    public TextMeshProUGUI stackCounter;
    public GameObject tooltipPrefab;
    public GameObject tooltipInstance;

    public void Start(){
        reference = new ArgumentCoreAi();       // TODO: Remove
        
        // Choose the tooltip to load when this argument is hovered (core argument/non-core argument tooltip)
        tooltipPrefab = (reference.isCore) ? Resources.Load("Prefabs/ArgumentTooltip") as GameObject : Resources.Load("Prefabs/ArgumentTooltip") as GameObject;

        stackCounter = transform.Find("StackCount").GetComponent<TextMeshProUGUI>();
        stackCounter.text = "x" + reference.stacks;         // TODO: Update this when a card is played rather than just on start.
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if (tooltipInstance == null){
            tooltipInstance = (GameObject) Instantiate(tooltipPrefab, transform.position + new Vector3(400, 0, 0), Quaternion.identity);
            tooltipInstance.SetActive(false);       // Workaround for now; force an OnEnable() call AFTER we load in data for the tooltip.
            tooltipInstance.GetComponent<TooltipArgument>().argumentRef = reference;
            tooltipInstance.transform.SetParent(gameObject.transform);
            tooltipInstance.SetActive(true);        // Forces an OnEnable() call so that the tooltip has the information (instead of nullref to AbstractArgument)
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        Destroy(tooltipInstance);
    }
}
