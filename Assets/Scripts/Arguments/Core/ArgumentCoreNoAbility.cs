using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreNoAbility : AbstractArgument
{
    public ArgumentCoreNoAbility(){
        this.ID = "CORE_NO_ABILITY";
        this.NAME = "";
        this.DESC = "This core argument has no special abilities.";
        this.OWNER = FactionType.ENEMY;
        this.ORIGIN = ArgumentOrigin.DEPLOYED;

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
    }
}