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

    public void TriggerEvent(AbstractEvent ae){
        NotifySubscribers(ae);
    }

    // TODO: Have this prevent double subscription based on an argument ID instead of checking for argument instances, to handle argument duplicates
    public void SubscribeToEvent(EventSubscriber subscriber, EventType type){
        // Debug.Log("Attempting to subscribe " + subscriber.INSTANCE_ID + " to " + type.GetType());
        // List<EventSubscriber> listOfSubscribers = gameEvents[type];
        // for (int i = 0; i < listOfSubscribers.Count; i++){
        //     if (listOfSubscribers[i].INSTANCE_ID == subscriber.INSTANCE_ID){
        //         return;
        //     }
        // }
        if (!subscriber.eventsSubscribedTo.Contains(type)){         // prevent double subscription
            subscriber.eventsSubscribedTo.Add(type);
            gameEvents[type].Add(subscriber);
        }
    }

    public void SubscribeToEvent(ITriggerOnEvent subscriber, EventType type){
        UIEvents[type].Add(subscriber);
    }

    private void UnsubscribeFromEvent(EventSubscriber subscriber, EventType type){
        // for (int i = gameEvents[type].Count - 1; i >= 0; i--){
        //     if (gameEvents[type][i].INSTANCE_ID == subscriber.INSTANCE_ID){
        //         gameEvents[type].RemoveAt(i);
        //     }
        // }
        gameEvents[type].RemoveAll(sub => sub.Equals(subscriber));
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

    private void NotifySubscribers(AbstractEvent eventData){
        List<EventSubscriber> subscribers = gameEvents[eventData.type];
        for (int cnt = subscribers.Count - 1; cnt >= 0; cnt--){     // Trigger buffs, then argument effects, then debuffs
            // This check is LITERALLY for only one status effect - Resonance.
            // Because Resonance will remove all other Resonance subscribers during its own NotifyOfEvent, and the original notification happens here...
            // If there were 5 resonance listeners, the subscribers.count would go from 5->0 in the first iteration, but the for loop would still iterate 4 more times.
            // This causes an array-out-of-index error. Ideally, this check prevents that.
            // I don't want to have to keep doing hacks like this as it's gonna come back to bite me, but it works.
            // TODO: Rework Resonance implementation to prevent having to use this
            if (cnt >= subscribers.Count){
                continue;
            }
            EventSubscriber subscriber = subscribers[cnt];
            subscriber.NotifyOfEvent(eventData);
        }
        
        // update ui
        List<ITriggerOnEvent> UISubscribers = UIEvents[eventData.type];
        foreach (ITriggerOnEvent subscriber in UISubscribers){
            subscriber.TriggerOnEvent(eventData);
        }
    }
}