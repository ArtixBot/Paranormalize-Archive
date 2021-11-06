using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgumentCoreAi : AbstractArgument
{
    public ArgumentCoreAi(){
        this.ID = "CORE_AI";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/combative");

        this.curHP = 26;
        this.maxHP = 26;
        this.stacks = 1;
        this.isCore = true;
    }
}