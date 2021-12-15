using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRest : AbstractStory
{
    public static string storyID = "REST_STORY";
    private static string filepath = "restPoint";       // don't need to use .json ending

    public StoryRest() : base(storyID, filepath, 0){
    }

    public override void SetupStory(){
        base.SetupStory();

        this._inkStory.BindExternalFunction("restoreResolve", () => {
            GameState.mainChar.coreArgument.curHP += 15;
            if (GameState.mainChar.coreArgument.curHP > GameState.mainChar.coreArgument.maxHP){
                GameState.mainChar.coreArgument.curHP = GameState.mainChar.coreArgument.maxHP;
            }
        });

        this._inkStory.BindExternalFunction("gainMastery", () => {
            GameState.mastery += 1;
        });
    }
}
