using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardViewOverlay : MonoBehaviour
{
    public RenderRewardMoney rewardTextMoney;
    public RenderRewardMastery rewardTextMastery;

    public int moneyToReward;
    public int masteryToReward;

    void OnEnable(){
        rewardTextMoney = GameObject.FindObjectOfType<RenderRewardMoney>();
        rewardTextMastery = GameObject.FindObjectOfType<RenderRewardMastery>();

        moneyToReward = 100;
        masteryToReward = 1;
        
        rewardTextMoney.CallCoroutine(moneyToReward);
        rewardTextMastery.CallCoroutine(masteryToReward);
    }
}
