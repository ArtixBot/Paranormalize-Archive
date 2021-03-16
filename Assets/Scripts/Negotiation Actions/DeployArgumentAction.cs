using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deploy an argument.
// Specifically - search to see if the argument exists. If so, add stacks to it.
// Else create a new argument of the type.
public class DeployArgumentAction : AbstractAction {

    public AbstractArgument argumentToDeploy;
    public int stacksToDeploy;
    public DeployArgumentAction(AbstractCharacter source, AbstractArgument argumentToDeploy, int stacksToDeploy){
        this.source = source;
        this.argumentToDeploy = argumentToDeploy;
        this.stacksToDeploy = stacksToDeploy;
        
        argumentToDeploy.OWNER = source;
        argumentToDeploy.stacks = this.stacksToDeploy;
    }

    public override void Resolve(){
        AbstractArgument instance = this.source.GetArgument(argumentToDeploy);
        if (instance != null){
            instance.stacks += stacksToDeploy;
        } else {
            this.source.nonCoreArguments.Add(argumentToDeploy);
        }
    }
}