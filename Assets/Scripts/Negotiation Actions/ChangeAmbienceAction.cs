using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ChangeAmbienceAction : AbstractAction {

    private int delta;

    ///<summary>
    ///Change the Ambience level.
    ///<list type="bullet">
    ///<item><term>delta</term><description>Change ambience by [delta] levels. Negative is in the direction of Guarded; Positive is in the direction of aggression.</description></item>
    ///</list>
    ///</summary>
    public ChangeAmbienceAction(int delta){
        this.delta = delta;
    }

    public override int Resolve(){
        NegotiationManager.Instance.ambience.AdjustState(delta);
        return 0;
    }
}