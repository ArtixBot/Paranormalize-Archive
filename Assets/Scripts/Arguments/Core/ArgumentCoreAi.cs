using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreAi : AbstractArgument
{
    public ArgumentCoreAi(){
        this.ID = "CORE_AI";
        this.NAME = "Combative";
        this.DESC = "Whenever one of Ai's arguments is destroyed, deal 1 damage to a random enemy argument.";
        this.OWNER = FactionType.PLAYER;
        this.ORIGIN = ArgumentOrigin.DEPLOYED;

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
    }
}