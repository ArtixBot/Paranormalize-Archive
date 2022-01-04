using System;
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
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        AbstractArgument instance = this.owner.GetArgument(argumentToPlant);
        if (instance == null || this.plantNewCopy){
            AbstractArgument newInstance = Activator.CreateInstance(this.argumentToPlant.GetType()) as AbstractArgument;
            newInstance.ORIGIN = ArgumentOrigin.PLANTED;
            newInstance.OWNER = this.owner;
            newInstance.stacks = this.stacksToPlant;
            newInstance.INSTANCE_ID = newInstance.ID + "_" + NegotiationManager.Instance.argumentsDeployedThisNegotiation;
            NegotiationManager.Instance.argumentsDeployedThisNegotiation += 1;

            this.owner.GetArguments().Add(newInstance);
            newInstance.TriggerOnDeploy();     // Add event subscriptions
            EventSystemManager.Instance.TriggerEvent(new EventArgCreated(newInstance));
        } else {
            Debug.Log("Copy exists; adding " + stacksToPlant + " stacks to it. Instance had " + instance.stacks + " stacks before adding these new ones.");
            instance.stacks += stacksToPlant;
            EventSystemManager.Instance.TriggerEvent(new EventArgStacksAdded(instance, stacksToPlant));
            Debug.Log("Instance now has " + instance.stacks + " stacks.");
        }
        return this.stacksToPlant;
    }
}