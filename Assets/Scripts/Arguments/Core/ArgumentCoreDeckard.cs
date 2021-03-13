using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreDeckard : AbstractArgument
{
    public ArgumentCoreDeckard(){
        this.ID = "CORE_DECKARD";
        this.NAME = "Collected";
        this.DESC = "At the start of Deckard's turn, this argument gains <style=\"Scalable\">1</style> <b>Poise</b>.";
        this.OWNER = FactionType.PLAYER;
        this.ORIGIN = ArgumentOrigin.DEPLOYED;

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
        this.isCore = true;
    }
}