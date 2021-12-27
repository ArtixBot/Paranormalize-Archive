using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;

public class DeckardAnger : AbstractCard {

    public static string cardID = "DECKARD_ANGER";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 1;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 5;
    public int STACKS = 1;

    public DeckardAnger() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));

        AmbienceState curState = NegotiationManager.Instance.ambience.GetState();
        if (curState == AmbienceState.AGITATED || curState == AmbienceState.VOLATILE || curState == AmbienceState.DANGEROUS){
            NegotiationManager.Instance.AddAction(new DeployArgumentAction(source, new ArgumentHeated(), STACKS));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.STACKS += 1;
    }
}