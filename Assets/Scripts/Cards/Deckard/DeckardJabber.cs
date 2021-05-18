using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardJabber : AbstractCard {

    public static string cardID = "DECKARD_JABBER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 2;
    public int MAX_DAMAGE = 4;
    public int POISE = 2;

    public DeckardJabber() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, MIN_DAMAGE, MAX_DAMAGE));
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), POISE));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
        this.POISE += 2;
    }
}