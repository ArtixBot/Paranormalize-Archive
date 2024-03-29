using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button codexButton;
    public Button settingsButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        playButton = transform.Find("PlayButton").gameObject.GetComponent<Button>();
        playButton.onClick.AddListener(PlayGame);

        exitButton = transform.Find("ExitButton").gameObject.GetComponent<Button>();
        exitButton.onClick.AddListener(ExitGame);
    }

    void PlayGame(){
        StartCoroutine(LoadOverworld());
    }

    private IEnumerator LoadOverworld(){        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Overworld");

        while (!asyncLoad.isDone){
            yield return null;
        }
    }

    void ExitGame(){
        Application.Quit();
    }
}
