using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - All event subscribers currently have a 'global' scope, i.e. they trigger whenever ANYONE triggers an EventType.
// Need to make it so we can trigger certain subscribers - e.g. only have an effect when the OTHER player plays a Dialogue card (not any player).
// Manages all negotiation gameEvents, like notify on argument destroyed.
// Arguments and relics can subscribe to gameEvents and then activate when their subscribed event triggers.
// In *general*, calls to EventSystemManager.Instance should run after all other effects occur.
// Singleton.
public class EventSystemManager
{
    public static readonly EventSystemManager Instance = new EventSystemManager();
    private Dictionary<EventType, List<EventSubscriber>> gameEvents = new Dictionary<EventType, List<EventSubscriber>>();
    private Dictionary<EventType, List<ITriggerOnEvent>> UIEvents = new Dictionary<EventType, List<ITriggerOnEvent>>();

    private EventSystemManager(){
        foreach (EventType type in Enum.GetValues(typeof(EventType))){
            gameEvents[type] = new List<EventSubscriber>();
            UIEvents[type] = new List<ITriggerOnEvent>();
        }
    }

    private void NotifySubscribers(AbstractEvent eventData){
        List<EventSubscriber> subscribers = gameEvents[eventData.type];
        // Debug.Log("EventSystemManager.cs: " + eventData.type.ToString() + " triggered; notifying " + subscribers.Count + " subscriber(s) to that event type");
        foreach (EventSubscriber subscriber in subscribers){
            subscriber.NotifyOfEvent(eventData);
        }

        // update ui
        List<ITriggerOnEvent> UISubscribers = UIEvents[eventData.type];
        foreach (ITriggerOnEvent subscriber in UISubscribers){
            subscriber.TriggerOnEvent(eventData);
        }
    }

    public void TriggerEvent(AbstractEvent ae){
        NotifySubscribers(ae);
    }

    public void SubscribeToEvent(EventSubscriber subscriber, EventType type){
        if (!subscriber.eventsSubscribedTo.Contains(type)){
            subscriber.eventsSubscribedTo.Add(type);
            gameEvents[type].Add(subscriber);
        }
    }

    public void SubscribeToEvent(ITriggerOnEvent subscriber, EventType type){
        UIEvents[type].Add(subscriber);
    }

    public void UnsubscribeFromEvent(EventSubscriber subscriber, EventType type){
        subscriber.eventsSubscribedTo.Remove(type);
        gameEvents[type].Remove(subscriber);
    }

    public void ClearAllSubscribers(){
        foreach (EventType type in Enum.GetValues(typeof(EventType))){
            foreach (EventSubscriber sub in gameEvents[type]){
                sub.eventsSubscribedTo.Clear();
            }
        }
        foreach (EventType type in Enum.GetValues(typeof(EventType))){
            gameEvents[type].Clear();
        }
    }
}