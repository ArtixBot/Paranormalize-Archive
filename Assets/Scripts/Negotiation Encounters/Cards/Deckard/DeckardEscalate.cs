using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardEscalate : AbstractCard {

    public static string cardID = "DECKARD_ESCALATE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MIN_DAMAGE = 3;
    public int MAX_DAMAGE = 5;
    public int BONUS = 2;

    public DeckardEscalate() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.AGGRESSION,
        CardRarity.UNCOMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);
        AmbienceState state = NegotiationManager.Instance.ambience.GetState();

        if (state == AmbienceState.DANGEROUS){
            NegotiationManager.Instance.AddAction(new DamageAction(target.OWNER.GetCoreArgument(), target.OWNER, MIN_DAMAGE + BONUS, MAX_DAMAGE + BONUS, this));
            List<AbstractArgument> nonCoreArgs = target.OWNER.GetTargetableArguments();
            foreach(AbstractArgument arg in nonCoreArgs){
                NegotiationManager.Instance.AddAction(new DamageAction(arg, arg.OWNER, MIN_DAMAGE +BONUS, MAX_DAMAGE + BONUS, this));
            }
        } else if (state == AmbienceState.VOLATILE || state == AmbienceState.AGITATED){
            NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE + BONUS, MAX_DAMAGE + BONUS, this));
        } else {
            NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
    }

    public override void Upgrade(){
        base.Upgrade();
        this.BONUS += 1;
    }
}