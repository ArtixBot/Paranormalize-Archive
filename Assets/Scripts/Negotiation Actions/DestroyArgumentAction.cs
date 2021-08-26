using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

// Destroy an argument.
// Specifically - search to see if the argument exists. If so, destroy it.
public class DestroyArgumentAction : AbstractAction {

    private AbstractCharacter owner;
    private AbstractArgument argumentToDestroy;
    
    public DestroyArgumentAction(AbstractArgument argumentToDestroy){
        this.argumentToDestroy = argumentToDestroy;
        this.owner = argumentToDestroy.OWNER;
    }

    public override int Resolve(){
        AbstractArgument instance = owner.GetArgument(argumentToDestroy);
        if (instance != null){
            EventSystemManager.Instance.TriggerEvent(new EventArgDestroyed(argumentToDestroy));    // trigger on-destroy effects (if any)
            this.argumentToDestroy.TriggerOnDestroy();                             // Remove event subscriptions and handle victory/defeat if a core argument was destroyed
            owner.nonCoreArguments.Remove(this.argumentToDestroy);                 // remove argument from the list of arguments (previous line will return if it's a core argument so no worries)
        }
        return 0;
    }
}