using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RenderRewardMastery : MonoBehaviour
{
    public TextMeshProUGUI text;

    void OnEnable(){
        text = GetComponent<TextMeshProUGUI>();
    }

    public void CallCoroutine(int mastery){
        StartCoroutine(MakeMasteryCounterGoBrr(mastery));
    }

    public IEnumerator MakeMasteryCounterGoBrr(int mastery = 0){
        while (text == null){
            yield return null;
        }
        float duration = 0.5f;
        float currentTime = 0f;
        while (currentTime <= duration){
            currentTime += Time.deltaTime;
            float normalized = Math.Min(currentTime / duration, 1.0f);
            int displayValue = (int)(mastery * normalized);
            text.text = "<color=#4ED667>+" + displayValue + "</color>";
            yield return null;
        }
        text.text = "<color=#4ED667>+" + mastery + "</color>";
        yield return null;
    }
}
