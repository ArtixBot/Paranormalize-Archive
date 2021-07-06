using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardCurveball : AbstractCard {

    public static string cardID = "DECKARD_CURVEBALL";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 3;
    public int DRAW = 2;

    public DeckardCurveball() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.ATTACK,
        new List<CardTags>{CardTags.PIERCING}
    ){
    }

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        if (NegotiationManager.Instance.numCardsPlayedThisTurn > 0){        // handle case if Curveball is the first card played this turn
            AbstractCard prevCard = NegotiationManager.Instance.cardsPlayedThisTurn.Last();
            if (prevCard.AMBIENCE == CardAmbient.DIALOGUE){
                NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this, true));
            } else if (prevCard.AMBIENCE == CardAmbient.INFLUENCE){
                NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
                NegotiationManager.Instance.AddAction(new DrawCardsAction(source, DRAW));
            } else {
                NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
            }
            return;
        }
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
        this.MAX_DAMAGE += 2;
    }
}