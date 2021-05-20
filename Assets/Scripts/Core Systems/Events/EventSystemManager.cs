using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - All event subscribers currently have a 'global' scope, i.e. they trigger whenever ANYONE triggers an EventType.
// Need to make it so we can trigger certain subscribers - e.g. only have an effect when the OTHER player plays a Dialogue card (not any player).
// Manages all negotiation events, like notify on argument destroyed.
// Arguments and relics can subscribe to events and then activate when their subscribed event triggers.
// In *general*, calls to EventSystemManager.Instance should run after all other effects occur.
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

    private void NotifySubscribers(AbstractEvent eventData){
        List<EventSubscriber> subscribers = events[eventData.type];
        Debug.Log(eventData.type.ToString() + " triggered; notifying " + subscribers.Count + " subscriber(s) to that event type");
        foreach (EventSubscriber subscriber in subscribers){
            subscriber.NotifyOfEvent(eventData);
        }
    }

    public void TriggerEvent(AbstractEvent ae){
        NotifySubscribers(ae);      // TODO: Replace second param with actual value!
    }

    public void SubscribeToEvent(EventSubscriber subscriber, EventType type){
        subscriber.eventsSubscribedTo.Add(type);
        events[type].Add(subscriber);
    }

    public void UnsubscribeFromEvent(EventSubscriber subscriber, EventType type){
        events[type].Remove(subscriber);
    }
}