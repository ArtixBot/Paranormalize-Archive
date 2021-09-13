using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

// Plant an argument.
// Specifically - search to see if the argument exists. If so, add stacks to it.
// Else create a new argument of the type.
public class PlantArgumentAction : AbstractAction {

    private AbstractCharacter owner;
    private AbstractArgument argumentToPlant;
    private int stacksToPlant;
    
    public PlantArgumentAction(AbstractCharacter target, AbstractArgument argumentToPlant, int stacksToPlant){
        this.owner = target;
        this.argumentToPlant = argumentToPlant;
        this.stacksToPlant = stacksToPlant;
        
        argumentToPlant.ORIGIN = ArgumentOrigin.PLANTED;
        argumentToPlant.OWNER = target;
        argumentToPlant.stacks = this.stacksToPlant;
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        AbstractArgument instance = this.owner.GetArgument(argumentToPlant);
        if (instance != null){
            Debug.Log("Copy exists; adding " + stacksToPlant + " stacks to it. Instance had " + instance.stacks + " stacks before adding these new ones.");
            instance.stacks += stacksToPlant;
            Debug.Log("Instance now has " + instance.stacks + " stacks.");
        } else {
            this.owner.nonCoreArguments.Add(argumentToPlant);
            argumentToPlant.TriggerOnDeploy();     // Add event subscriptions
            EventSystemManager.Instance.TriggerEvent(new EventArgCreated(argumentToPlant));
        }
        return this.stacksToPlant;
    }
}