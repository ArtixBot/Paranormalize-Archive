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

    void OnEnable(){        // editor bug causes this to get called TWICE in the editor (but works fine when building and running, feelsbad)
        rewardTextMoney = GameObject.FindObjectOfType<RenderRewardMoney>();
        rewardTextMastery = GameObject.FindObjectOfType<RenderRewardMastery>();
        draft = gameObject.GetComponentInChildren<DisplayDrafts>();

        draft.cardsToDraft = listOfDraftCards;

        skipDraftButton = GameObject.Find("Panel/SkipDraftButton").GetComponent<Button>();
        skipDraftButton.onClick.AddListener(FinishRewardView);

        rewardTextMoney.CallCoroutine(moneyToReward);
        rewardTextMastery.CallCoroutine(masteryToReward);
        draft.Render();
    }

    void OnDisable(){
        RenderNegotiation renderer = GameObject.Find("RenderNegotiation").GetComponent<RenderNegotiation>();
        renderer.EndNegotiationRender();
    }

    // private void Render(){
    //     // Debug.Log($"Awarding {moneyToReward} to player");
    //     rewardTextMoney.CallCoroutine(moneyToReward); 
    //     rewardTextMastery.CallCoroutine(masteryToReward);
    // }

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
