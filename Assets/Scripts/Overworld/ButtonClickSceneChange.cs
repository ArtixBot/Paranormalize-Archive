using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClickSceneChange : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EncounterInfo encounterInfo;
    public AbstractCharacter enemyIfNegotiationEncounter;   // enemy to use if an enemy encounter
    public string storyID;      // story ID to use if a story encounter

    public Image waypointImage;

    public void Render(){
        if (encounterInfo == null){
            return;
        }
        waypointImage = transform.Find("Image").GetComponentInChildren<Image>();
        Debug.Log(encounterInfo.encounterType);
        switch (encounterInfo.encounterType){
            case EncounterType.ENEMY:
                waypointImage.sprite = Resources.Load<Sprite>("Images/Overworld/normal-encounter");
                break;
            case EncounterType.ELITE:
                waypointImage.sprite = Resources.Load<Sprite>("Images/Overworld/elite-encounter");
                break;
            case EncounterType.BOSS:
                waypointImage.sprite = Resources.Load<Sprite>("Images/Overworld/elite-encounter");
                break;
            case EncounterType.EVENT:
            
            case EncounterType.SHOP:
            
            case EncounterType.REST:
                waypointImage.sprite = Resources.Load<Sprite>("Images/Overworld/rest-point");
                break;
            case EncounterType.FERRYMAN:
            
            default:
                waypointImage.sprite = Resources.Load<Sprite>("Images/missing");
                break;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData){
        GameState.currentStage += 1;
        OverworldManager.Instance.EncounterSelected(this.encounterInfo);
        switch (encounterInfo.encounterType){
            case EncounterType.ENEMY:
            case EncounterType.ELITE:
            case EncounterType.BOSS:
                StartCoroutine(LoadNegotiation());
                break;
                // enemy elite and boss encounters all switch the scene to negotiation
            case EncounterType.EVENT:
                StartCoroutine(LoadStory("TEST_STORY"));
                break;
            case EncounterType.FERRYMAN:
                StartCoroutine(LoadStory("FERRYMAN_STORY"));
                break;
            case EncounterType.REST:
                StartCoroutine(LoadStory("REST_STORY"));
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData){

    }

    public void OnPointerExit(PointerEventData pointerEventData){
        
    }

    private IEnumerator LoadNegotiation(){
        GameState.rngSeed = Random.Range(0, 1000000000);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Negotiation");

        while (!asyncLoad.isDone){
            yield return null;
        }
    }

    private IEnumerator LoadStory(string storyID){
        GameState.currentStoryID = storyID;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Story");

        while (!asyncLoad.isDone){
            yield return null;
        }
    }
}
