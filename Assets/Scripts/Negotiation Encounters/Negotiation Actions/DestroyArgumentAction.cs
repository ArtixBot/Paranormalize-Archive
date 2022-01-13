using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

// Destroy an argument.
// Specifically - search to see if the argument exists. If so, destroy it.
public class DestroyArgumentAction : AbstractAction {

    private AbstractCharacter owner;
    private AbstractArgument argumentToDestroy;
    private AbstractCard destroyedByThisCard;           // one of destroyedByThisCard/destroyedByThisArgument will be null. Use the non-null value for event throws so that subscribers can see what caused the destruction.
    private AbstractArgument destroyedByThisArgument;
    
    public DestroyArgumentAction(AbstractArgument argumentToDestroy){
        this.argumentToDestroy = argumentToDestroy;
        this.owner = argumentToDestroy.OWNER;
    }

    public DestroyArgumentAction(AbstractArgument argumentToDestroy, AbstractCard destroyingCard){
        this.argumentToDestroy = argumentToDestroy;
        this.owner = argumentToDestroy.OWNER;
        this.destroyedByThisCard = destroyingCard;
    }

    public DestroyArgumentAction(AbstractArgument argumentToDestroy, AbstractArgument destroyingArg){
        this.argumentToDestroy = argumentToDestroy;
        this.owner = argumentToDestroy.OWNER;
        this.destroyedByThisArgument = destroyingArg;
    }

    public override int Resolve(){
        AbstractArgument instance = owner.GetArgument(argumentToDestroy);
        if (instance != null){
            if (destroyedByThisCard != null){
                EventSystemManager.Instance.TriggerEvent(new EventArgDestroyed(argumentToDestroy, destroyedByThisCard));
            } else if (destroyedByThisArgument != null){
                EventSystemManager.Instance.TriggerEvent(new EventArgDestroyed(argumentToDestroy, destroyedByThisArgument));
            } else {
                EventSystemManager.Instance.TriggerEvent(new EventArgDestroyed(argumentToDestroy));    // trigger on-destroy effects (if any on the argument itself or on the destroying card/arg)
            }
            this.argumentToDestroy.TriggerOnDestroy();                             // Remove event subscriptions and handle victory/defeat if a core argument was destroyed
            
            owner.GetArguments().Remove(this.argumentToDestroy);                 // remove argument from the list of arguments (previous line will return if it's a core argument so no worries)
            // Remove all actions that were in the action queue and associated w/ the destroyed argument
            NegotiationManager.Instance.actionQueue.RemoveAll(action => action.origin == instance);
        }
        return 0;
    }
}