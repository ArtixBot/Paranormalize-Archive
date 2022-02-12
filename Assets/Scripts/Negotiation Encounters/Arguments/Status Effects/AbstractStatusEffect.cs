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
        this.stacks = stacks;

        if (type == StatusEffectType.BUFF){
            this.LISTENER_TYPE = EventListenerType.STATUS_BUFF;
        } else {
            this.LISTENER_TYPE = EventListenerType.STATUS_DEBUFF;
        }

    }

    public virtual void TriggerOnEffectApplied(){}

    public virtual void TriggerOnEffectExpire(){}

    public void ExpireEffect(){     // Immediately remove the current status effect.
        EventSystemManager.Instance.UnsubscribeFromAllEvents(this);     // Unsubscribe this from EventSystemManager
        this.TriggerOnEffectExpire();                                   // Trigger expiration effects (mostly just cleanup effects)
        this.host.statusEffects.Remove(this);                           // Remove from the list of status effects
    }
}
