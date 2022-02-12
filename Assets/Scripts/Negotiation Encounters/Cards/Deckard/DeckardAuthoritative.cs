using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardAuthoritative : AbstractCard {

    public static string cardID = "DECKARD_AUTHORITATIVE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 4;
    public int MAX_DAMAGE = 7;

    public DeckardAuthoritative() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.RARE,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int bonus = 0;
        List<AbstractArgument> arguments = source.GetSupportArguments();
        for (int i = 0; i < arguments.Count; i++){
            bonus += arguments[i].stacks;
        }
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE + bonus, MAX_DAMAGE + bonus, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 4;
        this.MAX_DAMAGE += 1;
    }
}