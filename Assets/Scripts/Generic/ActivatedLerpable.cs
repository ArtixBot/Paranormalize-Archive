using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedLerpable : MonoBehaviour
{
    public Vector3 origPos;        // original position
    public Vector3 currPos;        // current position
    public Vector3 targetPos;      // target position

    public float onActivateXOffset = 0f;
    public float onActivateYOffset = 0f;
    public float lerpDuration = 0.0f;

    // Start is called before the first frame update
    void Start(){
        origPos = gameObject.transform.position;
        currPos = gameObject.transform.position;
        targetPos = gameObject.transform.position;        
    }

    public void Offset(){
        targetPos = new Vector3(currPos.x + onActivateXOffset, currPos.y + onActivateYOffset, currPos.z);
        StartCoroutine(MoveTo(currPos, targetPos, lerpDuration));
    }

    public void ResetPosition(){
        targetPos = origPos;
        StartCoroutine(MoveTo(currPos, targetPos, lerpDuration));
    }

    private IEnumerator MoveTo(Vector3 oldPos, Vector3 newPos, float duration){
        float timeElapsed = 0;
        while (timeElapsed < duration){
            float t = timeElapsed / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            gameObject.transform.position = Vector3.Lerp(oldPos, newPos, t);
            timeElapsed += Time.deltaTime;
            currPos = gameObject.transform.position;

            yield return null;
        }
        gameObject.transform.position = newPos;
        currPos = gameObject.transform.position;
    }
}
