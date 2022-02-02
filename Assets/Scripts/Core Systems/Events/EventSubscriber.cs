using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When events are triggered, trigger events in the following order:
// Buff event listeners are first
// Debuff event listeners are next
// Then trigger all other event listeners - cards, arguments, etc.
public enum EventListenerType {DEFAULT, STATUS_BUFF, STATUS_DEBUFF}
public class EventSubscriber
{
    public EventListenerType LISTENER_TYPE = EventListenerType.DEFAULT;
    public List<EventType> eventsSubscribedTo = new List<EventType>();
    
    // Triggered by EventSystemManager.
    // Can be overwritten by any arguments/relics to perform behavior.
    public virtual void NotifyOfEvent(AbstractEvent eventData){}      
}
