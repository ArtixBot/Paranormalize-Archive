using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DisplayAmbience : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Ambience reference = NegotiationManager.Instance.ambience;

    public GameObject tooltipPrefab;
    public GameObject tooltipInstance;
    // public Image healthBarImageFill;
    // public TextMeshProUGUI stackCounter;
    // public TextMeshProUGUI healthBarText;

    public void OnEnable(){        
        tooltipPrefab = Resources.Load("Prefabs/AmbienceTooltip") as GameObject;
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if (tooltipInstance == null){
            int y = -100;
            tooltipInstance = (GameObject) Instantiate(tooltipPrefab, transform.position + new Vector3(0, y, 0), Quaternion.identity);
            tooltipInstance.SetActive(false);       // Workaround for now; force an OnEnable() call AFTER we load in data for the tooltip.
            tooltipInstance.transform.SetParent(GameObject.Find("Canvas").transform);
            tooltipInstance.SetActive(true);        // Forces an OnEnable() call so that the tooltip has the information (instead of nullref to AbstractArgument)
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        Destroy(tooltipInstance);
    }
}
