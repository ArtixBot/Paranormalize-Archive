using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardFilibuster : AbstractCard {

    public static string cardID = "DECKARD_FILIBUSTER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 3;

    public int POISE = 10;


    public DeckardFilibuster() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.INFLUENCE,
        CardRarity.RARE,
        CardType.SKILL,
        new List<CardTags>{CardTags.POISE, CardTags.SCOUR}
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, source.GetCoreArgument(), POISE));
        foreach(AbstractArgument arg in source.GetTargetableArguments()){
            NegotiationManager.Instance.AddAction(new ApplyPoiseAction(source, arg, POISE));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.POISE += 4;
    }
}