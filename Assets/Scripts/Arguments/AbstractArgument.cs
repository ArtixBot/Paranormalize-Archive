using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArgumentOrigin {DEPLOYED, PLANTED};
public abstract class AbstractArgument
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
    public int defense;     // Poise value
    public int stacks;      // Stack count
    public bool isCore = false; // Is a core argument (default false)
    // public List<ArgumentMods> modifiers;     // handle argument modifiers like Silenced

    public virtual void TriggerOnDeploy(){}
    public virtual void TriggerOnStartTurn(){}
    public virtual void TriggerOnEndTurn(){}
    public virtual void TriggerOnCardPlayed(){}
    public virtual void TriggerOnAmbienceShift(){}
    public virtual void TriggerOnDestroy(){
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
