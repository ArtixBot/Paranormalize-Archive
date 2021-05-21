using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deploy an argument.
// Specifically - search to see if the argument exists. If so, add stacks to it.
// Else create a new argument of the type.
public class DeployArgumentAction : AbstractAction {

    private AbstractCharacter owner;
    private AbstractArgument argumentToDeploy;
    private int stacksToDeploy;
    
    public DeployArgumentAction(AbstractCharacter source, AbstractArgument argumentToDeploy, int stacksToDeploy){
        this.owner = source;
        this.argumentToDeploy = argumentToDeploy;
        this.stacksToDeploy = stacksToDeploy;
        
        argumentToDeploy.ORIGIN = ArgumentOrigin.DEPLOYED;
        argumentToDeploy.OWNER = source;
        argumentToDeploy.stacks = this.stacksToDeploy;
    }

    ///<returns>An integer of how many stacks were added.</returns>
    public override int Resolve(){
        AbstractArgument instance = this.owner.GetArgument(argumentToDeploy);
        if (instance != null){
            instance.stacks += stacksToDeploy;
        } else {
            this.owner.nonCoreArguments.Add(argumentToDeploy);
            argumentToDeploy.TriggerOnDeploy();     // Add event subscriptions
            EventSystemManager.Instance.TriggerEvent(new EventArgCreated(argumentToDeploy));
        }
        return this.stacksToDeploy;
    }
}