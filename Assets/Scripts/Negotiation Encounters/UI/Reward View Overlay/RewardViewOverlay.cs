using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardViewOverlay : MonoBehaviour
{
    public Button skipDraftButton;
    public RenderRewardMoney rewardTextMoney;
    public RenderRewardMastery rewardTextMastery;

    private DisplayDrafts draft;

    public List<AbstractCard> listOfDraftCards;
    public int moneyToReward;
    public int masteryToReward;

    void OnEnable(){        // Unity bug causes this to get called TWICE when enabling a disabled game object -- wtf?
        rewardTextMoney = GameObject.FindObjectOfType<RenderRewardMoney>();
        rewardTextMastery = GameObject.FindObjectOfType<RenderRewardMastery>();
        draft = gameObject.GetComponentInChildren<DisplayDrafts>();

        draft.cardsToDraft = listOfDraftCards;

        skipDraftButton = GameObject.Find("Panel/SkipDraftButton").GetComponent<Button>();
        skipDraftButton.onClick.AddListener(FinishRewardView);

        draft.Render();
    }

    public void Render(){
        rewardTextMoney.CallCoroutine(moneyToReward);
        rewardTextMastery.CallCoroutine(masteryToReward);
    }

    private void FinishRewardView(){
        StartCoroutine(ReturnToOverworld());
    }

    private IEnumerator ReturnToOverworld(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Overworld");
        while (!asyncLoad.isDone){
            yield return null;
        }
    }
}
