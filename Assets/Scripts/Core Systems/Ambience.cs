using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

// The ambience level of the current negotiation.
// Can be changed by specific cards or by building enough Influence points in the Guarded/Dangerous direction.
public enum AmbienceState {GUARDED = -1, TENSE = 0, AGITATED = 1, VOLATILE = 2, DANGEROUS = 3}
public class Ambience
{
    public static readonly Ambience Instance = new Ambience();
    private bool stateCanChange = true;
    private int score = 0;
    private int MIN_THRESHOLD = -1;
    private int MAX_THRESHOLD = 3;

    private int shiftsToDiplomacyThisTurn = 0;
    private int shiftsToAggressionThisTurn = 0;


    public void AdjustState(int howMuch){
        if (!stateCanChange || howMuch == 0){return;}       // Don't bother with any of this if ambience cannot change
        AmbienceState oldState = GetState();

        // Change ambience score
        this.score += howMuch;

        // Make sure ambience score doesn't go outside thresholds, in case we try to shift the level towards Guarded when already Guarded (or towards Dangerous when already Dangerous)
        if (this.score < MIN_THRESHOLD){
            this.score = MIN_THRESHOLD;
        } else if (this.score > MAX_THRESHOLD){
            this.score = MAX_THRESHOLD;
        }

        if (oldState != GetState()){        // If ambience state changes...
            EventSystemManager.Instance.TriggerEvent(new EventAmbientStateShift(oldState, GetState()));
            if (oldState > GetState()){     // shifted towards diplomacy
                shiftsToDiplomacyThisTurn += 1;
            } else {    // shifted towards aggression
                shiftsToAggressionThisTurn += 1;
            }
        }
    }

    public AmbienceState GetState(){
        return (AmbienceState) score;
    }

    public void SetState(AmbienceState state){
        this.score = (int) state;
    }

    public void SetState(int state){
        this.score = state;
    }

    ///<summary>Return the number of times the ambience level shifted this turn.</summary>
    public int GetShiftsThisTurn(){
        return this.shiftsToAggressionThisTurn + this.shiftsToDiplomacyThisTurn;
    }
}