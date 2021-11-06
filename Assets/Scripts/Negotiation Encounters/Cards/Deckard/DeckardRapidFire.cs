using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckardRapidFire : AbstractCard {

    public static string cardID = "DECKARD_RAPID_FIRE";
    private static Dictionary<string, string> cardStrings = LocalizationLibrary.Instance.GetCardStrings(cardID);
    private static int cardCost = 0;

    public int MIN_DAMAGE = 1;
    public int MAX_DAMAGE = 3;
    public int STACKS = 1;

    public DeckardRapidFire() : base(
        cardID,
        cardStrings,
        cardCost,
        CardAmbient.DIALOGUE,
        CardRarity.COMMON,
        CardType.ATTACK
    ){}

    public override void Play(AbstractCharacter source, AbstractArgument target){
        base.Play(source, target);

        int bonus_attacks = 0;
        List<AbstractArgument> args = this.OWNER.GetArguments();
        for (int i = 0; i < args.Count; i++){
            if (args[i].ID == "CHATTERBOX"){
                bonus_attacks += args[i].stacks;
            }
        }
        // Debug.Log("Rapid Fire attacks " + (bonus_attacks + 1) + " time(s).");
        for (int i = 0; i < 1 + bonus_attacks; i++){
            NegotiationManager.Instance.AddAction(new DamageAction(target, target.OWNER, MIN_DAMAGE, MAX_DAMAGE, this));
        }
        NegotiationManager.Instance.AddAction(new DeployArgumentAction(this.OWNER, new ArgumentChatterbox(), STACKS));
    }

    public override void Upgrade(){
        base.Upgrade();
        this.MIN_DAMAGE += 1;
        this.MAX_DAMAGE += 1;
    }
}