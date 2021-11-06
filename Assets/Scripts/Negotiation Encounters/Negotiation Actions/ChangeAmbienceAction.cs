using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ChangeAmbienceAction : AbstractAction {

    private int delta;

    ///<summary>
    ///Change the Ambience level.
    ///<list type="bullet">
    ///<item><term>delta</term><description>Change ambience by [delta] levels. Negative is in the direction of Guarded; Positive is in the direction of Dangerous..</description></item>
    ///</list>
    ///</summary>
    public ChangeAmbienceAction(int delta){
        this.delta = delta;
    }

    public override int Resolve(){
        AmbienceState oldState = NegotiationManager.Instance.ambience.GetState();
        NegotiationManager.Instance.ambience.AdjustState(delta);
        AmbienceState newState = NegotiationManager.Instance.ambience.GetState();
        if (oldState != newState){
            EventSystemManager.Instance.TriggerEvent(new EventAmbientStateShift(oldState, newState));
        }
        return 0;
    }
}