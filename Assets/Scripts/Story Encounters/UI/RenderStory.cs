using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    [SerializeField]
    private GameObject optionsFeed;

	private GameObject playerModel;
	private GameObject enemyModel;
	private GameObject allyModel;
	private GameObject bystanderModel;

    public GameObject textPrefab;
    public Button choicePrefab;
    // Called on loading into Story scene
    void Start()
    {
        dialogFeed = transform.Find("DialogFeed/Viewport/Content").gameObject;
        optionsFeed = transform.Find("OptionsFeed").gameObject;

		playerModel = transform.Find("PlayerAvatar").gameObject;
		allyModel = transform.Find("AllyAvatar").gameObject;
		enemyModel = transform.Find("EnemyAvatar").gameObject;
		bystanderModel = transform.Find("BystanderAvatar").gameObject;

        textPrefab = Resources.Load("Prefabs/DialogBubble") as GameObject;
        // choicePrefab = Resources.Load("Prefabs/Button") as Button;

        if (storyToRender == null){
            Debug.Log("No valid story was supplied; supplying test story instead");
            storyToRender = new StoryTest();
        }
        storyToRender.SetupStory();
        story = storyToRender._inkStory;
        StartCoroutine(PlayStory());
    }

	IEnumerator PlayStory(){
		for (int i = optionsFeed.transform.childCount - 1; i >= 0; --i) {			// delete old buttons
			GameObject.Destroy (optionsFeed.transform.GetChild (i).gameObject);
		}
        while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			Debug.Log("Current line: " + text);
			HandleActionTags(story.currentTags);
            CreateDialogueBubble(text);
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
			yield return new WaitForSeconds(0.01f);		// for some reason, MouseButtonUp() will cause two dialogue bubbles to pop up (it must be getting counted as true over two frames?)
		}

        // Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
        // If we've read all the content and there's no choices, the story is finished!
		// else {
		// 	Button choice = CreateChoiceView("End of story.\nRestart?");
		// 	choice.onClick.AddListener(delegate{
		// 		PlayStory();
		// 	});
		// }
    }

    void CreateDialogueBubble(string text){
        GameObject bubble = Instantiate (textPrefab) as GameObject;
		if (CurrentStoryContainsTag("enemy")){
			bubble.GetComponent<Image>().color = new Color(1f, 0.75f, 0.75f);
		} else if (CurrentStoryContainsTag("ally")){
			bubble.GetComponent<Image>().color = new Color(0.75f, 1f, 0.75f);
		} else if (CurrentStoryContainsTag("narrator")){
			bubble.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
		} else {		// assume player
			bubble.GetComponent<Image>().color = new Color(0.75f, 0.75f, 1f, 0.75f);
		}
        bubble.GetComponent<DialogBubble>().textContent.text = text;
        bubble.transform.SetParent(dialogFeed.transform, false);
    }

    // When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
        StartCoroutine(PlayStory());
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (choicePrefab) as Button;
		choice.transform.SetParent(optionsFeed.transform, false);
		
		// Gets the text from the button prefab
		TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI> ();
		choiceText.text = text;

		return choice;
	}

	bool CurrentStoryContainsTag(string str){
		return story.currentTags.Contains(str);
	}

	void HandleActionTags(List<string> tagList){
		// Handles tags of the following format:
		// action:actor (actor performs the action)
		// List of supported actions: enter/exit
		// List of supported actors: player/ally/enemy/bystander
		for (int i = 0; i < tagList.Count; i++){
			bool performAction = tagList[i].Contains(":");
			if (!performAction){
				continue;
			}
			string whatIsAction = tagList[i].Split(':')[0];
			string whoIsActor = tagList[i].Split(':')[1];

			GameObject actor = null;
			switch(whoIsActor){
				case "player":
					actor = playerModel;
					break;
				case "ally":
					actor = allyModel;
					break;
				case "enemy":
					actor = enemyModel;
					break;
				case "bystander":
					actor = bystanderModel;
					break;
			}

			switch(whatIsAction){
				case "enter":
					if (actor.TryGetComponent(out ActivatedLerpable enter)){
						enter.Offset();
					}
					break;
				case "exit":
					if (actor.TryGetComponent(out ActivatedLerpable exit)){
						exit.ResetPosition();
					}
					break;
			}
		}
	}
}
