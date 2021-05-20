using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSubscriber
{
    // Triggered by EventSystemManager.
    // Can be overwritten by any arguments/relics to perform behavior.
    public virtual void NotifyOfEvent(AbstractEvent eventData){}      
}
