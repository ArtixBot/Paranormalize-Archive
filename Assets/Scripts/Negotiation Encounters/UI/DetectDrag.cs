using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LineRenderer lr;

    void OnEnable(){
        lr = gameObject.GetComponent<LineRenderer>();
    }
    
    public void OnBeginDrag(PointerEventData eventData){
        this.lr.enabled = true;
        this.lr.SetPosition(0, Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 0, 10));
        this.lr.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 0, 10));

        // transform.position += new Vector3(0, 100, 0);
    }

    public void OnDrag(PointerEventData eventData){
        this.lr.SetPosition(1, Camera.main.ScreenToWorldPoint(eventData.position) + new Vector3(0, 0, 10));
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(this.transform);
        this.lr.enabled = false;

        // transform.position -= new Vector3(0, 100, 0);
    }
}
