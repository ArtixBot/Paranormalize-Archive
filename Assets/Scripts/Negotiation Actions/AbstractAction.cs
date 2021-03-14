using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAction
{
    public AbstractCharacter source;
    public AbstractCharacter target;

    public abstract void Resolve();
}