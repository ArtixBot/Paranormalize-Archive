using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectDrop : MonoBehaviour, IDropHandler
{
    public AbstractArgument argRef;
    public GameObject render;

    void OnEnable(){
        argRef = gameObject.GetComponent<DisplayArgument>().reference;
        render = GameObject.Find("RenderNegotiation");
    }

    public void OnDrop(PointerEventData eventData){
        AbstractCard card = eventData.pointerDrag.GetComponent<DisplayCard>().reference;
        if (card != null){
            try {
                NegotiationManager.Instance.PlayCard(card, TurnManager.Instance.GetCurrentCharacter(), argRef);
                // Debug.Log("DetectDrop.cs: Played " + card.NAME + " on " + argRef.OWNER.NAME +"'s " + argRef.NAME);
                render.GetComponent<RenderNegotiation>().RenderHand();
            } catch (Exception ex) {
                Debug.LogWarning("DetectDrop.cs failed to play card, reason: " + ex.Message);
            }
        } else {
            Debug.LogWarning("Failed to play card, card is null!");
        }
    }
}
