using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTest : AbstractStory
{
    public static string storyID = "TEST_STORY";
    private static string filepath = "teststory";       // don't need to use .json ending

    public StoryTest() : base(storyID, filepath){}
}
