using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentTalismanResolve : AbstractArgument
{
    public ArgumentTalismanResolve(){
        this.ID = "TALISMAN_RESOLVE";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/talisman-renewal");

        this.curHP = 3;
        this.maxHP = 3;
        this.stacks = 1;
    }
}
