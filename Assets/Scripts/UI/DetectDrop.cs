using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectDrop : MonoBehaviour, IDropHandler
{
    public AbstractArgument argRef;
    public GameObject handDisplay;

    void OnEnable(){
        argRef = gameObject.GetComponent<DisplayArgument>().reference;
        handDisplay = GameObject.Find("HandZone");
    }

    public void OnDrop(PointerEventData eventData){
        AbstractCard card = eventData.pointerDrag.GetComponent<DisplayCard>().reference;
        if (card != null){
            try {
                NegotiationManager.Instance.PlayCard(card, TurnManager.Instance.GetCurrentCharacter(), argRef);
                Debug.Log("DetectDrop.cs: Played " + card.NAME + " on " + argRef.OWNER.NAME +"'s " + argRef.NAME);
                // handDisplay.GetComponent<HandDisplay>().DisplayHand();
            } catch (Exception ex) {
                Debug.LogWarning("Failed to play card, reason: " + ex.Message);
            }
        }
    }
}
