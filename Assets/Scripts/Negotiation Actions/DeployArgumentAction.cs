using System;
using UnityEngine;
using GameEvent;

// Deploy an argument.
// Specifically - search to see if the argument exists. If so, add stacks to it.
// Else create a new argument of the type.
public class DeployArgumentAction : AbstractAction {

    private AbstractCharacter owner;
    private AbstractArgument argumentToDeploy;
    private int stacksToDeploy;
    private bool deployNewCopy;
    
    public DeployArgumentAction(AbstractCharacter source, AbstractArgument argumentToDeploy, int stacksToDeploy, bool deployNewCopy = false){
        this.owner = source;
        this.argumentToDeploy = argumentToDeploy;
        this.stacksToDeploy = stacksToDeploy;
        this.deployNewCopy = deployNewCopy;     // if true, deploy a new copy of the argument even if one already exists
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        AbstractArgument instance = this.owner.GetArgument(this.argumentToDeploy);
        if (instance == null || this.deployNewCopy){
            // Create a new instance of the argument and set its appropriate values.
            AbstractArgument newInstance = Activator.CreateInstance(this.argumentToDeploy.GetType()) as AbstractArgument;
            newInstance.ORIGIN = ArgumentOrigin.DEPLOYED;
            newInstance.OWNER = this.owner;
            newInstance.stacks = this.stacksToDeploy;
            newInstance.INSTANCE_ID = newInstance.ID + "_" + NegotiationManager.Instance.argumentsDeployedThisNegotiation;
            NegotiationManager.Instance.argumentsDeployedThisNegotiation += 1;

            this.owner.nonCoreArguments.Add(newInstance);
            newInstance.TriggerOnDeploy();     // Add event subscriptions
            EventSystemManager.Instance.TriggerEvent(new EventArgCreated(newInstance));
        } else {
            Debug.Log("Copy exists; adding " + stacksToDeploy + " stacks to it. Instance had " + instance.stacks + " stacks before adding these new ones.");
            instance.stacks += stacksToDeploy;
            Debug.Log("Instance now has " + instance.stacks + " stacks.");
        }
        return this.stacksToDeploy;
    }
}