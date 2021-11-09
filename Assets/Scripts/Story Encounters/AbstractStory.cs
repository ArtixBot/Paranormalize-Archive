using UnityEngine;  
using UnityEngine.UI;
using System;
using Ink.Runtime;

// This is a super bare bones example of how to play and display a ink story in Unity.
public abstract class AbstractStory {
    public static event Action<Story> OnCreateStory;

    public string STORY_ID;         // Story ID
    public TextAsset inkAsset;      // ink .json compiled file
    public Story _inkStory;         // .ink file. Derives automatically from inkAsset.

    public AbstractStory(string ID, string textAssetPath){
        this.STORY_ID = ID;
        this.inkAsset = Resources.Load<TextAsset>("Ink/" + textAssetPath);
        Debug.Log("Ink/" + textAssetPath);
        // this._inkStory = new Story(this.inkAsset.text);
    }
	
    // void Awake () {
	// 	// Remove the default message
	// 	RemoveChildren();
	// 	SetupStory();
	// }

	// Creates a new Story object with the compiled story which we can then play!
	public void SetupStory () {
		_inkStory = new Story (inkAsset.text);
        if(OnCreateStory != null) OnCreateStory(_inkStory);
		_inkStory.variablesState["player"] = (GameState.mainChar != null) ? GameState.mainChar.NAME : "Player";
        _inkStory.variablesState["ally"] = (GameState.ally != null) ? GameState.ally.NAME : "Ally";

		// RefreshView();
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	// void RefreshView () {
	// 	// Remove all the UI on screen
	// 	RemoveChildren ();
		
	// 	// Read all the content until we can't continue any more
	// 	while (story.canContinue) {
	// 		// Continue gets the next line of the story
	// 		string text = story.Continue ();
	// 		// This removes any white space from the text.
	// 		text = text.Trim();
	// 		// Display the text on screen!
	// 		CreateContentView(text);
	// 		Debug.Log("Current line: " + text + " has " + story.currentTags.Count + " tags");
	// 	}

	// 	// Display all the choices, if there are any!
	// 	if(story.currentChoices.Count > 0) {
	// 		for (int i = 0; i < story.currentChoices.Count; i++) {
	// 			Choice choice = story.currentChoices [i];
	// 			Button button = CreateChoiceView (choice.text.Trim ());
	// 			// Tell the button what to do when we press it
	// 			button.onClick.AddListener (delegate {
	// 				OnClickChoiceButton (choice);
	// 			});
	// 		}
	// 	}
	// 	// If we've read all the content and there's no choices, the story is finished!
	// 	else {
	// 		Button choice = CreateChoiceView("End of story.\nRestart?");
	// 		choice.onClick.AddListener(delegate{
	// 			SetupStory();
	// 		});
	// 	}
	// }

	// // When we click the choice button, tell the story to choose that choice!
	// void OnClickChoiceButton (Choice choice) {
	// 	story.ChooseChoiceIndex (choice.index);
	// 	RefreshView();
	// }

	// // Creates a textbox showing the the line of text
	// void CreateContentView (string text) {
	// 	// Text storyText = Instantiate (textPrefab) as Text;
	// 	// storyText.text = text;
	// 	// storyText.transform.SetParent (canvas.transform, false);
	// }

	// // Creates a button showing the choice text
	// Button CreateChoiceView (string text) {
	// 	// Creates the button from a prefab
	// 	// Button choice = Instantiate (buttonPrefab) as Button;
	// 	// choice.transform.SetParent (canvas.transform, false);
		
	// 	// // Gets the text from the button prefab
	// 	// Text choiceText = choice.GetComponentInChildren<Text> ();
	// 	// choiceText.text = text;

	// 	// // Make the button expand to fit the text
	// 	// HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
	// 	// layoutGroup.childForceExpandHeight = false;

	// 	// return choice;
    //     return null;
	// }

	// // Destroys all the children of this gameobject (all the UI)
	// void RemoveChildren () {
	// 	int childCount = canvas.transform.childCount;
	// 	for (int i = childCount - 1; i >= 0; --i) {
	// 		GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
	// 	}
	// }

	// [SerializeField]
	// private TextAsset inkJSONAsset = null;
	// public Story story;

	// [SerializeField]
	// private Canvas canvas = null;

	// // UI Prefabs
	// [SerializeField]
	// private Text textPrefab = null;
	// [SerializeField]
	// private Button buttonPrefab = null;
}
