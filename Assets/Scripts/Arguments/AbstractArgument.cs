using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArgumentOrigin {DEPLOYED, PLANTED};
public abstract class AbstractArgument : EventSubscriber
{

    // Cosmetic
    public string NAME;
    public string DESC;
    public Sprite IMG;

    // Gameplay
    public string ID;
    public AbstractCharacter OWNER;
    public ArgumentOrigin ORIGIN;

    public int maxHP;       // Maximum resolve
    public int curHP;       // Current resolve
    public int poise;     // Poise value
    public int stacks;      // Stack count
    public bool isCore = false; // Is a core argument (default false)
    public bool isTrait = false;    // Is a trait (default false). Traits cannot be destroyed.
    // public List<ArgumentMods> modifiers;     // handle argument modifiers like Silenced

    public virtual void TriggerOnDeploy(){}     // Subscribe to all relevant events by overriding this function.
    public virtual void TriggerOnDestroy(){     // Win/lose if it's a core argument.
        if (this.isCore){
            if (this.OWNER.FACTION == FactionType.PLAYER){
                NegotiationManager.Instance.EndNegotiationLost();
                return;
            } else {
                NegotiationManager.Instance.EndNegotiationWon();
                return;
            }
        }

        // Automatically unsubscribe from all relevant events (a list of event types is maintained in eventsSubscribedTo, which is inherited from EventSubscriber)
        foreach (EventType type in this.eventsSubscribedTo){
            EventSystemManager.Instance.UnsubscribeFromEvent(this, type);
        }
    }

    public bool IsDeployed(){
        return this.ORIGIN == ArgumentOrigin.DEPLOYED;
    }

    public bool IsPlanted(){
        return this.ORIGIN == ArgumentOrigin.PLANTED;
    }
}
