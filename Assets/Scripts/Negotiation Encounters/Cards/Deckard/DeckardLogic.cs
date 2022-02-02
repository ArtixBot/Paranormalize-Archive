using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardLogic : AbstractCard {

    public static string cardID = "DECKARD_LOGIC";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int STACKS = 1;

    public DeckardLogic() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.SKILL
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        if (target.isCore){
            throw new System.Exception("Cannot target core arguments with Logic!");
        }
        if (target.OWNER == this.OWNER){
            NegotiationManager.Instance.AddAction(new AddStacksToArgumentAction(target, STACKS));
        } else {
            NegotiationManager.Instance.AddAction(new AddStacksToArgumentAction(target, -STACKS));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}