﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreNoAbility : AbstractArgument
{
    public ArgumentCoreNoAbility(){
        this.ID = "CORE_NO_ABILITY";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.OWNER = FactionType.ENEMY;
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/boring");

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
        this.isCore = true;
    }
}