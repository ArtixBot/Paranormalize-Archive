using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreHardenedResolve : AbstractArgument
{
    public ArgumentCoreHardenedResolve(){
        this.ID = "CORE_HARDENED_RESOLVE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/combative");

        this.curHP = 30;
        this.maxHP = 30;
        this.stacks = 1;
        this.isCore = true;
    }
}