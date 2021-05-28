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