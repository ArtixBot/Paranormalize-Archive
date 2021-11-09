using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class RenderStory : MonoBehaviour
{
    public AbstractStory storyToRender;
    public GameObject textPrefab;
    public GameObject choicePrefab;
    // Called on loading into Story scene
    void Start()
    {
        if (storyToRender == null){
            Debug.Log("No valid story was supplied, so supplying test story instead");
            storyToRender = new StoryTest(); 
        }
        // storyToRender.SetupStory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
