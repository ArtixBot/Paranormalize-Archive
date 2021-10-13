using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAction
{
    
    // optional value, that, if set, indicates that this action was added to the queue by an argument.
    // This is so that if an argument is destroyed, we remove all actions linked to this argument from the action queue.
    public AbstractArgument origin;     

    public abstract int Resolve();
}