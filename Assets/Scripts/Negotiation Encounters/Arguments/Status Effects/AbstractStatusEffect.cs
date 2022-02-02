using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType {BUFF = 1, DEBUFF = 2}
public abstract class AbstractStatusEffect : EventSubscriber{

    public string ID;
    public string DESC;
    public string NAME;
    public StatusEffectType TYPE;

    public AbstractArgument host;       // Set automatically from ApplyStatusEffectAction.cs
    public int stacks;                  // Very few (if any arguments) use stacks; only Parapsych, really

    public AbstractStatusEffect(string ID, Dictionary<string, string> strings, StatusEffectType type, int stacks = 0){
        this.ID = ID;
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.TYPE = type;

        if (type == StatusEffectType.BUFF){
            this.LISTENER_TYPE = EventListenerType.STATUS_BUFF;
        } else {
            this.LISTENER_TYPE = EventListenerType.STATUS_DEBUFF;
        }

        this.stacks = stacks;
    }

    public virtual void TriggerOnEffectApplied(){}      // Subscribe to all relevant events and edit the values of the argument by overriding this function.

    public virtual void TriggerOnEffectExpire(){}

    public virtual void TriggerOnHostDies(){    // Unsubscribe from any events (if relevant). Should also undo any changes made here.
        EventSystemManager.Instance.UnsubscribeFromAllEvents(this);
    }
}
