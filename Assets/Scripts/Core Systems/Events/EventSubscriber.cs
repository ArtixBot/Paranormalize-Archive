using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSubscriber
{
    public string INSTANCE_ID;      // should be unique. assigned to both cards and arguments
    public List<EventType> eventsSubscribedTo = new List<EventType>();
    
    // Triggered by EventSystemManager.
    // Can be overwritten by any arguments/relics to perform behavior.
    public virtual void NotifyOfEvent(AbstractEvent eventData){}      
}
