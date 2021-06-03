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
        if (eventData.pointerDrag.GetComponent<DisplayCard>() == null){return;}
        AbstractCard card = eventData.pointerDrag.GetComponent<DisplayCard>().reference;
        if (card != null){
            // Debug.Log("DetectDrop.cs: Played " + card.NAME + " on " + argRef.OWNER.NAME +"'s " + argRef.NAME);
            NegotiationManager.Instance.PlayCard(card, TurnManager.Instance.GetCurrentCharacter(), argRef);
            render.GetComponent<RenderNegotiation>().RenderHand();
            render.GetComponent<RenderNegotiation>().RenderNonCoreArguments();
            render.GetComponent<RenderNegotiation>().RenderCounts();
            return;
        }
        Debug.LogWarning("Failed to play card, card is null!");
    }
}
