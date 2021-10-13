using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all negotiation game events, like notify on argument destroyed.
// Cards, artifacts, arguments, and the negotiation manager itself use gameEvents.
// UI elements inherit from MonoBehavior and thus use ITriggerOnEvent instead -- they are stored in UIEvents.
// gameEvents update first, then UIEvents update afterwards.
// Singleton.
public class EventSystemManager
{
    public static readonly EventSystemManager Instance = new EventSystemManager();
    public Dictionary<EventType, List<EventSubscriber>> gameEvents = new Dictionary<EventType, List<EventSubscriber>>();
    private Dictionary<EventType, List<ITriggerOnEvent>> UIEvents = new Dictionary<EventType, List<ITriggerOnEvent>>();

    private EventSystemManager(){
        foreach (EventType type in Enum.GetValues(typeof(EventType))){
            gameEvents[type] = new List<EventSubscriber>();
            UIEvents[type] = new List<ITriggerOnEvent>();
        }
    }

    private void NotifySubscribers(AbstractEvent eventData){
        List<EventSubscriber> subscribers = gameEvents[eventData.type];
        for (int i = subscribers.Count - 1; i >= 0; i--){
            EventSubscriber subscriber = subscribers[i];
            subscriber.NotifyOfEvent(eventData);
        }

        // update ui
        List<ITriggerOnEvent> UISubscribers = UIEvents[eventData.type];
        foreach (ITriggerOnEvent subscriber in UISubscribers){
            subscriber.TriggerOnEvent(eventData);
        }
    }

    public void TriggerEvent(AbstractEvent ae){
        // Debug.Log("Triggered event: " + ae.GetType());
        NotifySubscribers(ae);
    }

    // TODO: Have this prevent double subscription based on an argument ID instead of checking for argument instances, to handle argument duplicates
    public void SubscribeToEvent(EventSubscriber subscriber, EventType type){
        if (!subscriber.eventsSubscribedTo.Contains(type)){         // prevent double subscription
            subscriber.eventsSubscribedTo.Add(type);
            gameEvents[type].Add(subscriber);
        }
    }

    public void SubscribeToEvent(ITriggerOnEvent subscriber, EventType type){
        UIEvents[type].Add(subscriber);
    }

    private void UnsubscribeFromEvent(EventSubscriber subscriber, EventType type){
        gameEvents[type].Remove(subscriber);
    }

    public void UnsubscribeFromEvent(ITriggerOnEvent subscriber, EventType type){
        UIEvents[type].Remove(subscriber);
    }

    public void UnsubscribeFromAllEvents(EventSubscriber subscriber){
        foreach (EventType type in subscriber.eventsSubscribedTo){
            EventSystemManager.Instance.UnsubscribeFromEvent(subscriber, type);
        }
        subscriber.eventsSubscribedTo.Clear();
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