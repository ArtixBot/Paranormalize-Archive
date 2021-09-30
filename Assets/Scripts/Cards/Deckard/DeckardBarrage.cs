using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardBarrage : AbstractCard {

    public static string cardID = "DECKARD_BARRAGE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int MIN_DAMAGE = 4;
    public int MAX_DAMAGE = 4;

    private bool addOne = false;

    public DeckardBarrage() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK,
        new List<CardTags>{CardTags.POISE}
    ){
        this.COSTS_ALL_AP = true;
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        int cnt = (addOne) ? 1 + source.curAP : source.curAP;
        for (int i = 0; i < cnt; i++){
            NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAmbientShiftAggression(), cnt));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.addOne = true;
    }
}