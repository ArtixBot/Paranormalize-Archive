using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class SetAmbienceAction : AbstractAction {

    private int state;

    ///<summary>
    ///Set the Ambience level.
    ///<list type="bullet">
    ///<item><term>state</term><description>Change ambience to [state]. Review Ambience.cs to view the enum values assigned to each level.</description></item>
    ///</list>
    ///</summary>
    public SetAmbienceAction(int state){
        this.state = state;
    }

    public SetAmbienceAction(AmbienceState state){
        this.state = (int)state;
    }

    public override int Resolve(){
        AmbienceState oldState = NegotiationManager.Instance.ambience.GetState();
        NegotiationManager.Instance.ambience.SetState(this.state);
        AmbienceState newState = NegotiationManager.Instance.ambience.GetState();
        if (oldState != newState){
            EventSystemManager.Instance.TriggerEvent(new EventAmbientStateShift(oldState, newState));
        }
        return 0;
    }
}