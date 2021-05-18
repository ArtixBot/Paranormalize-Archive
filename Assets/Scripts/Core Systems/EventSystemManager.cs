using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

// TODO - All event subscribers currently have a 'global' scope, i.e. they trigger whenever ANYONE triggers an EventType.
// Need to make it so we can trigger certain subscribers - e.g. only have an effect when the OTHER player plays a Dialogue card (not any player).
// Manages all negotiation events, like notify on argument destroyed.
// Arguments and relics can subscribe to events and then activate when their subscribed event triggers.
// Singleton.
public class EventSystemManager
{
    public static readonly EventSystemManager Instance = new EventSystemManager();
    private Dictionary<EventType, List<EventSubscriber>> events = new Dictionary<EventType, List<EventSubscriber>>();

    private EventSystemManager(){
        foreach (EventType type in Enum.GetValues(typeof(EventType))){
            events[type] = new List<EventSubscriber>();
        }
    }

    private void NotifySubscribers(EventType type, AbstractCharacter characterWhoTriggeredEvent){
        List<EventSubscriber> subscribers = events[type];
        Debug.Log(type.ToString() + " triggered; notifying " + subscribers.Count + " subscriber(s) to that event type");
        foreach (EventSubscriber subscriber in subscribers){
            subscriber.Notify();
        }
    }

    public void TriggerEvent(EventType type){
        NotifySubscribers(type, null);      // TODO: Replace second param with actual value!
    }

    public void SubscribeToEvent(EventSubscriber subscriber, EventType type){
        events[type].Add(subscriber);
    }

    public void UnsubscribeFromEvent(EventSubscriber subscriber, EventType type){
        events[type].Remove(subscriber);
    }
}