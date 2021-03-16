using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    void Start(){
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyUp(KeyCode.E)){
            NegotiationManager.Instance.NextTurn();
            //StartCoroutine(LoadYourAsyncScene());
        }
        if (Input.GetKeyUp(KeyCode.Q)){
        }
    }

    IEnumerator LoadYourAsyncScene(){
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = (SceneManager.GetActiveScene().name == "Test") ? SceneManager.LoadSceneAsync("Negotiation") : SceneManager.LoadSceneAsync("Test");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}