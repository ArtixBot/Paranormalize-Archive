using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all negotiation events, like notify on argument destroyed.
// Arguments and relics can subscribe to events and then activate when their subscribed event triggers.
// Singleton.
public enum EventType {
    ARGUMENT_CREATED,
    ARGUMENT_DESTROYED,
    ARGUMENT_POISE_DESTROYED,
    CARD_PLAYED,                    // events like DIALOGUE_CARD_PLAYED or AGGRESSION_CARD_PLAYED should also trigger CARD_PLAYED.
    DIALOGUE_CARD_PLAYED,
    AGGRESSION_CARD_PLAYED,
    INFLUENCE_CARD_PLAYED,
    ATTACK_CARD_PLAYED,
    SKILL_CARD_PLAYED,
    TRAIT_CARD_PLAYED
};

public class EventSystemManager
{
    public static readonly EventSystemManager Instance = new EventSystemManager();
    private Dictionary<EventType, EventType> events = new Dictionary<EventType, EventType>();

    private EventSystemManager(){
        // foreach (EventType event in EventType){

        // }
    }
}