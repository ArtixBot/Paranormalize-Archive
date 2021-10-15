using System.Collections;
using System.Collections.Generic;
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
        
        argumentToDeploy.ORIGIN = ArgumentOrigin.DEPLOYED;
        argumentToDeploy.OWNER = source;
        argumentToDeploy.stacks = this.stacksToDeploy;
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        AbstractArgument instance = this.owner.GetArgument(argumentToDeploy);
        if (instance == null || this.deployNewCopy){
            this.owner.nonCoreArguments.Add(argumentToDeploy);
            argumentToDeploy.TriggerOnDeploy();     // Add event subscriptions
            EventSystemManager.Instance.TriggerEvent(new EventArgCreated(argumentToDeploy));
            argumentToDeploy.INSTANCE_ID = argumentToDeploy.ID + "_" + NegotiationManager.Instance.argumentsDeployedThisNegotiation;
            NegotiationManager.Instance.argumentsDeployedThisNegotiation += 1;
            Debug.Log("Deploying " + argumentToDeploy.INSTANCE_ID);
        } else {
            Debug.Log("Copy exists; adding " + stacksToDeploy + " stacks to it. Instance had " + instance.stacks + " stacks before adding these new ones.");
            instance.stacks += stacksToDeploy;
            Debug.Log("Instance now has " + instance.stacks + " stacks.");
        }
        return this.stacksToDeploy;
    }
}