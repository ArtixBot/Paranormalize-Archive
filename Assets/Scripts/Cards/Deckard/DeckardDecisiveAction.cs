using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardDecisiveAction : AbstractCard {

    public static string cardID = "DECKARD_DECISIVE_ACTION";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int ACTIONS = 2;

    public DeckardDecisiveAction() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.UNCOMMON,
        CardType.SKILL
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);

        this.OWNER.GetCoreArgument().poise = 0;
        foreach(AbstractArgument arg in this.OWNER.GetArguments()){
            arg.poise = 0;
        }
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentInept(), 1));
        this.OWNER.curAP += this.ACTIONS;
    }

    public override void Upgrade(){
        base.Upgrade();
        this.ACTIONS += 1;
    }
}