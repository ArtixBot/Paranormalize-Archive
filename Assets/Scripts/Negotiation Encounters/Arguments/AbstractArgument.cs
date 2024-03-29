﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArgumentOrigin {DEPLOYED, PLANTED};
public abstract class AbstractArgument : EventSubscriber
{
    // Cosmetic constants
    public string NAME;
    public string DESC;
    public Sprite IMG;

    // Gameplay constants
    public string ID;
    public string INSTANCE_ID;
    public AbstractCharacter OWNER;
    public ArgumentOrigin ORIGIN;

    // Stat values
    public int maxHP;       // Maximum resolve
    public int curHP;       // Current resolve
    public int poise;     // Poise value
    public int stacks;      // Stack count
    public bool isPriorityTarget = false;  // If true, enemies will target this argument first (default false). Use for decoy arguments.
    public bool isCore = false; // Is a core argument (default false)
    public bool isTrait = false;    // Is a trait (default false). Traits cannot be destroyed.

    // Stat modifiers
    public float dmgTakenMult = 1.0f;       // This argument takes [dmgTakenMult]x damage (default 1.0x)
    public int dmgTakenAdd = 0;           // This argument takes +[dmgTakenAdd] damage (default 0).
    
    public List<AbstractStatusEffect> statusEffects = new List<AbstractStatusEffect>{};     // handle argument modifiers like Silenced

    public virtual void TriggerOnDeploy(){}     // Subscribe to all relevant events by overriding this function.

    public virtual void TriggerOnDestroy(){     // Win/lose if it's a core argument.
        EventSystemManager.Instance.UnsubscribeFromAllEvents(this);
        for(int i = statusEffects.Count - 1; i >= 0; i--){
            statusEffects[i].ExpireEffect();
        }
        if (this.isCore){
            if (this.OWNER.FACTION == FactionType.PLAYER){
                NegotiationManager.Instance.EndNegotiationLost();
                return;
            } else {
                NegotiationManager.Instance.EndNegotiationWon();
                return;
            }
        }
    }

    public bool IsDeployed(){
        return this.ORIGIN == ArgumentOrigin.DEPLOYED;
    }

    public bool IsPlanted(){
        return this.ORIGIN == ArgumentOrigin.PLANTED;
    }
}
