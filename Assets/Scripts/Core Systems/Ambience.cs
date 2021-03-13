using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The ambience level of the current negotiation.
public enum AmbienceState {DIPLOMATIC, TEMPERED, GUARDED, TENSE, AGITATED, VOLATILE, DANGEROUS}
public class Ambience
{
    public static readonly Ambience Instance = new Ambience();
    public AmbienceState state;
    public int value = 0;
}