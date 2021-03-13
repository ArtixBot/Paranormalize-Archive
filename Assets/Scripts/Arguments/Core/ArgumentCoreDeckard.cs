using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreDeckard : AbstractArgument
{

    public ArgumentCoreDeckard(){
        this.ID = "CORE_DECKARD";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.OWNER = FactionType.PLAYER;
        this.ORIGIN = ArgumentOrigin.DEPLOYED;

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
        this.isCore = true;
    }
}