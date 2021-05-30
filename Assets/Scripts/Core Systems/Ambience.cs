using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

// The ambience level of the current negotiation.
// Represented by a 'score' that ranges from [-8, 18]. Playing a Diplomacy card adds -1, an Aggression card +1 by default.

// When changing ambience state, add 2 "barrier points" which makes it harder to change ambience in the opposite direction again in that turn.
// Subsequent state changes in the same turn will add the same amount of barrier points plus two
// (so 4/6/8/... "barrier points" if we've changed ambience 2/3/4/... times this turn)
// Barrier points are removed before changing score, at a rate of one point per score change prevention.
// All barrier points are removed at the end of the current player's turn.
public enum AmbienceState {GUARDED = -1, TENSE = 0, AGITATED = 1, VOLATILE = 2, DANGEROUS = 3}
public class Ambience
{
    public static readonly Ambience Instance = new Ambience();
    private bool scoreCanChange = true;
    private int score = 0;
    private int MIN_THRESHOLD = -8;
    private int MAX_THRESHOLD = 18;

    private int diplomacyBarrier = 0;
    private int aggressionBarrier = 0;
    private int shiftsToDiplomacyThisTurn = 0;
    private int shiftsToAggressionThisTurn = 0;

    public AmbienceState GetState(){
        if (this.score >= MIN_THRESHOLD && this.score < -4){
            return AmbienceState.GUARDED;
        } else if (this.score >= -4 && this.score < 5){
            return AmbienceState.TENSE;
        } else if (this.score >= 5 && this.score < 9){
            return AmbienceState.AGITATED;
        } else if (this.score >= 9 && this.score < 13){
            return AmbienceState.VOLATILE;
        } else if (this.score >= 13 && this.score < MAX_THRESHOLD){
            return AmbienceState.DANGEROUS;
        }
        return AmbienceState.TENSE;
    }

    public void AdjustState(int howMuch){
        if (!scoreCanChange || howMuch == 0){return;}       // Don't bother with any of this if ambience cannot change
        AmbienceState oldState = GetState();

        int value = howMuch;

        // Barrier points are consumed before the ambience score changes
        if (value < 0){
            if ( (Math.Abs(value) - diplomacyBarrier) > 0){
                value += diplomacyBarrier;
                diplomacyBarrier = 0;
            } else {
                diplomacyBarrier += value;
                return;
            }
        } else {
            if ( (value - aggressionBarrier) > 0){
                value -= aggressionBarrier;
                aggressionBarrier = 0;
            } else {
                aggressionBarrier -= value;
                return;
            }
        }

        // Change ambience score
        this.score += value;

        // Make sure ambience score doesn't go outside thresholds
        if (this.score < MIN_THRESHOLD){
            this.score = MIN_THRESHOLD;
        } else if (this.score > MAX_THRESHOLD){
            this.score = MAX_THRESHOLD;
        }

        if (oldState != GetState()){        // If ambience state changes...
            EventSystemManager.Instance.TriggerEvent(new EventAmbientStateShift(oldState, GetState()));
            if (oldState > GetState()){     // shifted towards diplomacy
                shiftsToDiplomacyThisTurn += 1;
                aggressionBarrier += 2 * shiftsToDiplomacyThisTurn;     // harder to turn back to aggression when you shift to diplomacy!
            } else {    // shifted towards aggression
                shiftsToAggressionThisTurn += 1;
                diplomacyBarrier += 2 * shiftsToAggressionThisTurn;     // vice versa also applies
            }
        }
    }

    public void SetState(AmbienceState state){
        this.score = 6 * (int)state;        // TODO: placeholder value
    }

    ///<summary>Return the number of times the ambience level shifted this turn.</summary>
    public int GetShiftsThisTurn(){
        return this.shiftsToAggressionThisTurn + this.shiftsToDiplomacyThisTurn;
    }
}