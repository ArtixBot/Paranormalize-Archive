using System;
using UnityEngine;
using GameEvent;

// Add stacks to an argument.
public class AddStacksToArgumentAction : AbstractAction {

    private AbstractArgument argumentToAddStacksTo;
    private int stacksToAdd;
    
    public AddStacksToArgumentAction(AbstractArgument argumentToAddStacksTo, int stacksToAdd){
        this.argumentToAddStacksTo = argumentToAddStacksTo;
        this.stacksToAdd = stacksToAdd;
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        if (argumentToAddStacksTo == null){
            return 0;
        }
        argumentToAddStacksTo.stacks += stacksToAdd;
        EventSystemManager.Instance.TriggerEvent(new EventArgStacksAdded(argumentToAddStacksTo, stacksToAdd));
        return this.stacksToAdd;
    }
}