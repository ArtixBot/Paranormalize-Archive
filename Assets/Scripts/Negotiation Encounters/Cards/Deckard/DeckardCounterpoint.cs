using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardCounterpoint : AbstractCard {

    public static string cardID = "DECKARD_COUNTERPOINT";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 4;
    public int MAX_DAMAGE = 6;
    public int INFLUENCE = 4;

    public DeckardCounterpoint() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        AmbienceState state = Ambience.Instance.GetState();
        bool bonusEffects = (state == AmbienceState.GUARDED || state == AmbienceState.TENSE);
        int multiplier = (bonusEffects) ? 2 : 1;
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE * multiplier, MAX_DAMAGE * multiplier, this));
        if (bonusEffects){
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentAmbientShiftAggression(), INFLUENCE));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 2;
    }
}