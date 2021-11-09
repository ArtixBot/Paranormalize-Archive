using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class RenderStory : MonoBehaviour
{
    public AbstractStory storyToRender;
    private Story story;

    [SerializeField]
    private GameObject dialogFeed;

    public GameObject textPrefab;
    public GameObject choicePrefab;
    // Called on loading into Story scene
    void Start()
    {
        dialogFeed = transform.Find("DialogFeed/Viewport/Content").gameObject;
        textPrefab = Resources.Load("Prefabs/DialogBubble") as GameObject;

        if (storyToRender == null){
            Debug.Log("No valid story was supplied; supplying test story instead");
            storyToRender = new StoryTest();
        }
        storyToRender.SetupStory();
        story = storyToRender._inkStory;
        PlayStory();
    }

    void PlayStory(){
        while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			Debug.Log("Current line: " + text);
            CreateDialogueBubble(text);
		}
    }

    void CreateDialogueBubble(string text){
        GameObject bubble = Instantiate (textPrefab) as GameObject;
        bubble.GetComponent<DialogBubble>().textContent.text = text;
        bubble.transform.SetParent(dialogFeed.transform, false);
		// storyText.transform.SetParent (canvas.transform, false);
    }
}
