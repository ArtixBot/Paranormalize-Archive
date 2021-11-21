using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRest : AbstractStory
{
    public static string storyID = "REST_STORY";
    private static string filepath = "restPoint";       // don't need to use .json ending

    public StoryRest() : base(storyID, filepath){}
}
