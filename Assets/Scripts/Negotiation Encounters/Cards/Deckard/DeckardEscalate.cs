using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardEscalate : AbstractCard {

    public static string cardID = "DECKARD_ESCALATE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 2;

    public int MULTIPLIER = 4;
    public int INFLUENCE = 3;

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

        if (target.isCore || target.OWNER != this.OWNER){
            throw new System.Exception("You must target friendly support arguments with Interrogate!");
        }
        int damageToDeal = target.stacks * MULTIPLIER;
        AbstractCharacter enemy = TurnManager.Instance.GetOtherCharacter(target.OWNER);

        // Destroy sacrifical argument
        NegotiationManager.Instance.AddAction(new DestroyArgumentAction(target));

        // Damage enemy arguments
        NegotiationManager.Instance.AddAction(new DamageAction(enemy.GetCoreArgument(), enemy, damageToDeal, damageToDeal, this));
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentAmbientShiftAggression(), INFLUENCE));

    }

    public override void Upgrade(){
        base.Upgrade();
        this.MULTIPLIER += 2;
    }
}