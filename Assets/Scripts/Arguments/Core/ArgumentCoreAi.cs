using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreAi : AbstractArgument
{
    public ArgumentCoreAi(){
        this.ID = "CORE_AI";
        this.NAME = "Combative";
        this.DESC = "Whenever one of Ai's arguments is destroyed, deal <style=\"Scalable\">1</style> damage to a random enemy argument.";
        this.OWNER = FactionType.PLAYER;
        this.ORIGIN = ArgumentOrigin.DEPLOYED;

        this.curHP = 26;
        this.maxHP = 26;
        this.stacks = 1;
        this.isCore = true;
    }
}