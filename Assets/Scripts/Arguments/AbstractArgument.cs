using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArgumentOrigin {DEPLOYED, PLANTED};
public abstract class AbstractArgument
{
    public string ID;
    public string NAME;
    public string DESC;
    public ArgumentOrigin ORIGIN;

    public int maxHP;      // Maximum resolve
    public int curHP;      // Current resolve
    public int defense;         // Poise value
    public int stacks;

    public bool IsDeployed(){
        return this.ORIGIN == ArgumentOrigin.DEPLOYED;
    }

    public bool IsPlanted(){
        return this.ORIGIN == ArgumentOrigin.PLANTED;
    }
}
