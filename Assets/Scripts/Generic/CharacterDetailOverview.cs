using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// used by our view deck button to enable/disable CharacterViewOverlay
public class CharacterDetailOverview : MonoBehaviour, IPointerClickHandler
{
    public GameObject instance;
    public bool prefabEnabled = false;

    public void OnPointerClick(PointerEventData eventData){
        if (prefabEnabled){
            instance.SetActive(false);
        } else {
            instance.SetActive(true);
        }
        prefabEnabled = !prefabEnabled;
    }
}
