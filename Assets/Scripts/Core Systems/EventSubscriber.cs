using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotifyType {
    NOTIFY_WHEN_TRIGGERED_BY_OWNER,         // Notify this event subscriber when the owner of this subscriber triggers an event.
    NOTIFY_WHEN_TRIGGERED_BY_NONOWNER,      // Notify this event subscriber when the non-owner of this subscriber triggers an event.
    NOTIFY_WHEN_TRIGGERED                   // Notify this event subscriber whenever an event is triggered.
}

public class EventSubscriber
{
    public NotifyType notifyType;
    // Triggered by EventSystemManager.
    // Can be overwritten by any arguments/relics to perform behavior.
    public virtual void Notify(){}      
}
