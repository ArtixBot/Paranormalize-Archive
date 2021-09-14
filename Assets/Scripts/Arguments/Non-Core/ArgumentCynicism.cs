using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class ArgumentCynicism : AbstractArgument
{
    public AbstractCard CARD_ONE;
    public AbstractCard CARD_TWO;

    public ArgumentCynicism(){
        this.ID = "CYNICISM";
        Dictionary<string, string> strings = LocalizationLibrary.Instance.GetArgumentStrings(this.ID);
        this.NAME = strings["NAME"];
        this.DESC = strings["DESC"];
        this.ORIGIN = ArgumentOrigin.DEPLOYED;
        this.IMG = Resources.Load<Sprite>("Images/Arguments/cynicism");

        this.stacks = 1;
    }

    public override void TriggerOnDeploy(){
        base.TriggerOnDeploy();
        CARD_ONE.COST += this.stacks;
        CARD_TWO.COST += this.stacks;
    }

    public override void TriggerOnDestroy(){
        CARD_ONE.COST -= this.stacks;
        CARD_TWO.COST -= this.stacks;
    }

    public override void NotifyOfEvent(AbstractEvent eventData){
        EventArgDestroyed data = (EventArgDestroyed) eventData;
        if (data.argumentDestroyed.OWNER != this.OWNER){
            this.OWNER.curAP += this.stacks;
            NegotiationManager.Instance.AddAction(new DrawCardsAction(this.OWNER, this.stacks));
        }
    }
}
