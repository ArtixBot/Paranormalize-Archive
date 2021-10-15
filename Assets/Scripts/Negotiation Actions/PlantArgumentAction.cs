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
    private bool plantNewCopy;
    
    public PlantArgumentAction(AbstractCharacter target, AbstractArgument argumentToPlant, int stacksToPlant, bool plantNewCopy = false){
        this.owner = target;
        this.argumentToPlant = argumentToPlant;
        this.stacksToPlant = stacksToPlant;
        this.plantNewCopy = plantNewCopy;     // if true, plant a new copy of the argument even if one already exists
        
        argumentToPlant.ORIGIN = ArgumentOrigin.PLANTED;
        argumentToPlant.OWNER = target;
        argumentToPlant.stacks = this.stacksToPlant;
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        AbstractArgument instance = this.owner.GetArgument(argumentToPlant);
        if (instance == null || this.plantNewCopy){
            this.owner.nonCoreArguments.Add(argumentToPlant);
            argumentToPlant.TriggerOnDeploy();     // Add event subscriptions
            EventSystemManager.Instance.TriggerEvent(new EventArgCreated(argumentToPlant));
            argumentToPlant.INSTANCE_ID = argumentToPlant.ID + "_" + NegotiationManager.Instance.argumentsDeployedThisNegotiation;
            NegotiationManager.Instance.argumentsDeployedThisNegotiation += 1;
            Debug.Log("Planting " + argumentToPlant.INSTANCE_ID);
        } else {
            Debug.Log("Copy exists; adding " + stacksToPlant + " stacks to it. Instance had " + instance.stacks + " stacks before adding these new ones.");
            instance.stacks += stacksToPlant;
            Debug.Log("Instance now has " + instance.stacks + " stacks.");
        }
        return this.stacksToPlant;
    }
}