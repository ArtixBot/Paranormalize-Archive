using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The ambience level of the current negotiation.
// Represented by a 'score' that ranges from [-8, 18]. Playing a Diplomacy card adds -1, an Aggression card +1 by default.

// When changing ambience state, add 2 "barrier points" which makes it harder to change ambience again in that turn.
// Subsequent state changes in the same turn will add the same amount of barrier points plus two
// (so 4/6/8/... "barrier points" if we've changed ambience 2/3/4/... times this turn)
// Barrier points are removed before changing score, at a rate of one point per score change prevention.
// All barrier points are removed at the end of the current player's turn.
public enum AmbienceState {GUARDED, TENSE, AGITATED, VOLATILE, DANGEROUS}
public class Ambience
{
    public static readonly Ambience Instance = new Ambience();
    public int score = 0;

    public AmbienceState GetState(){
        return AmbienceState.TENSE;
    }
}