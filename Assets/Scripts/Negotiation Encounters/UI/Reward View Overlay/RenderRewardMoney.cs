using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RenderRewardMoney : MonoBehaviour
{
    public TextMeshProUGUI text;

    void OnEnable(){
        text = GetComponent<TextMeshProUGUI>();
    }

    public void CallCoroutine(int money){
        StartCoroutine(MakeMoneyCounterGoBrr(money));
    }

    public IEnumerator MakeMoneyCounterGoBrr(int money = 100){
        while (text == null){
            yield return null;
        }
        float duration = 0.5f;
        float currentTime = 0f;
        while (currentTime <= duration){
            currentTime += Time.deltaTime;
            float normalized = Math.Min(currentTime / duration, 1.0f);
            int displayValue = (int)(money * normalized);
            text.text = "<color=#FFEA69>+" + displayValue + "</color>";
            yield return null;
        }
        text.text = "<color=#FFEA69>+" + money + "</color>";
        yield return null;
    }
}
