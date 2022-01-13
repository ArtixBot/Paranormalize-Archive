using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardTurnabout : AbstractCard {

    public static string cardID = "DECKARD_TURNABOUT";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public DeckardTurnabout() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.UNCOMMON,
        CardType.SKILL
    ){
        this.COSTS_ALL_AP = true;
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (target.isCore){
            throw new Exception("Can only target support arguments with Turnabout!");
        }
        int cnt = (this.isUpgraded) ? 1 + source.curAP : source.curAP;
        int stacksToGain = Math.Min(target.stacks, cnt);
        
        target.stacks -= cnt;
        if (target.stacks <= 0){            // Destroy the argument if it falls below 0 stacks
            target.stacks = 0;
            NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target, this));
        }

        List<AbstractArgument> recipients = this.OWNER.GetTargetableArguments();
        if (recipients.Count > 0){
            int index = UnityEngine.Random.Range(0, recipients.Count);
            NegotiationManager.Instance.AddAction(new AddStacksToArgumentAction(recipients[index], stacksToGain));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
    }
}