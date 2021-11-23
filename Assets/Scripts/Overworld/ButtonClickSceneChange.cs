using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonClickSceneChange : MonoBehaviour, IPointerClickHandler
{
    public EncounterType encounterType;
    public AbstractCharacter enemyIfNegotiationEncounter;   // enemy to use if an enemy encounter
    public string storyID;      // story ID to use if a story encounter

    public void OnPointerClick(PointerEventData pointerEventData){
        GameState.currentStage += 1;
        switch (encounterType){
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

    private IEnumerator LoadNegotiation(){
        GameState.randomSeed = Random.Range(0, 1000000000);
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
