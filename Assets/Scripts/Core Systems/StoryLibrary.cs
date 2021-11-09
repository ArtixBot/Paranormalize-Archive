using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

// StoryLibrary pairs card IDs with the actual Class Type. Singleton.
public class StoryLibrary
{
    public static readonly StoryLibrary Instance = new StoryLibrary();
    private Dictionary<string, Type> table = new Dictionary<string, Type>();

    private StoryLibrary(){
        float startUp = Time.realtimeSinceStartup;
        Assembly assembly = typeof(AbstractStory).Assembly;

        // Use reflection to grab all of the subclasses of AbstractStory.
        // Definitely eats performance. But this *should* only be called once at the start of the game?
        Type[] storyClasses = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractStory))).ToArray();        // Could also use t.baseClass == typeof(AbstractStory)
        foreach (Type story in storyClasses){
            // For each story that inherits from AbstractStory, create one instance of it to grab the ID so we can form an ID -> class pairing.
            AbstractStory instance = Activator.CreateInstance(story) as AbstractStory;
            table.Add(instance.STORY_ID, story);
        }
        float endStartUp = Time.realtimeSinceStartup;
        Debug.Log("StoryLibrary loaded definition of ALL stories; took " + (endStartUp - startUp) + " seconds.");
    }

    // Given a card's ID, return the appropriate class for that card.
    public Type Lookup(string key){
        try {
            return table[key];
        } catch (KeyNotFoundException){
            Debug.LogError("Key '" + key + "' was not found in StoryLibrary!");
            return null;
        }
    }
}