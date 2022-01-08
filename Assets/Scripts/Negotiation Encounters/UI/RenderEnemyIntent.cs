using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RenderEnemyIntent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    public EnemyIntent reference;
    public AbstractArgument targetToDrawLineTo;
    
    public Image image; 
    public LineRenderer lr;

    void OnEnable(){
        image = gameObject.GetComponent<Image>();
        lr = gameObject.GetComponent<LineRenderer>();

        image.sprite = Resources.Load<Sprite>("Images/Arguments/adaptive");
    }

    public void OnPointerEnter(PointerEventData eventData){
        GameObject argumentObject = null;
        targetToDrawLineTo = reference.argumentTargeted;
        if (targetToDrawLineTo.OWNER.FACTION == FactionType.PLAYER){
            if (targetToDrawLineTo.isCore){
                argumentObject = GameObject.Find("PlayerSide/SpawnCoreHere/ArgumentDisplay(Clone)");
            } else {

            }
        } else {
            if (targetToDrawLineTo.isCore){
                argumentObject = GameObject.Find("EnemySide/SpawnCoreHere/ArgumentDisplay(Clone)");
            } else {

            }
        }
        if (argumentObject != null){
            this.lr.enabled = true;
            this.lr.sortingOrder = 1;
            this.lr.material = new Material (Shader.Find ("Sprites/Default"));
            this.lr.material.color = Color.red; 
            this.lr.SetPosition(0, Camera.main.ScreenToWorldPoint(transform.position) + new Vector3(0, 0, 10));
            this.lr.SetPosition(1, Camera.main.ScreenToWorldPoint(argumentObject.transform.position) + new Vector3(0, 0, 10));
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        this.lr.enabled = false;
    }
}