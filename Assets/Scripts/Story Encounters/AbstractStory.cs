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
        this._inkStory = new Story(this.inkAsset.text);
    }

	// Creates a new Story object with the compiled story which we can then play!
	public virtual void SetupStory () {
		_inkStory = new Story (inkAsset.text);
        if(OnCreateStory != null) OnCreateStory(_inkStory);
		
        // set variable values
        if (_inkStory.variablesState["player"] != null){
            _inkStory.variablesState["player"] = (GameState.mainChar != null) ? GameState.mainChar.NAME : "Player";
        }
        if (_inkStory.variablesState["ally"] != null){
            _inkStory.variablesState["ally"] = (GameState.ally != null) ? GameState.ally.NAME : "Ally";
        }
	}
}
