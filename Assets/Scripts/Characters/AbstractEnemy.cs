using System.Collections.Generic;
using UnityEngine;

public class EnemyIntent {
    public AbstractCard cardToPlay;
    public AbstractArgument argumentTargeted;
    public IntentType intentType;

    public enum IntentType {
        ATTACK, BUFF_SKILL, DEBUFF_SKILL, TRAIT, UNKNOWN
    }

    public EnemyIntent(AbstractCard cardToPlay, AbstractArgument target){
        this.cardToPlay = cardToPlay;
        this.argumentTargeted = target;
        if (cardToPlay.TYPE == CardType.TRAIT){
            this.intentType = IntentType.TRAIT;
        } else if (cardToPlay.TYPE == CardType.ATTACK){
            this.intentType = IntentType.ATTACK;
        } else if (cardToPlay.TYPE == CardType.SKILL){
            if (target.OWNER == cardToPlay.OWNER){
                this.intentType = IntentType.BUFF_SKILL;
            } else {
                this.intentType = IntentType.DEBUFF_SKILL;
            }
        } else {
            this.intentType = IntentType.UNKNOWN;
        }
    }

    public void Resolve(){
        NegotiationManager.Instance.PlayCard(this.cardToPlay, this.cardToPlay.OWNER, this.argumentTargeted);
    }
}

public abstract class AbstractEnemy : AbstractCharacter {
    public AbstractEnemy(string ID, string NAME, AbstractArgument core, bool isPlayerFaction) : base(ID, NAME, core, isPlayerFaction){}
    
    public List<EnemyIntent> intents = new List<EnemyIntent>();

    // Call this at the end of enemy's turn/start of player's turn so that we can see their intents in advance!
    // Enemy AI uses weights to determine cards to play. Higher weights are always prioritized; cards in the same weight are then randomly selected.
    // Once a card is selected, choose a target. If an attack was selected, check to see if any enemy arguments have the MUST_TARGET flag to select that as the target, else choose randomly
    public void CalculateIntents(){
        intents.Clear();        // clear old intents
        List<AbstractCard> currentHand = new List<AbstractCard>(this.GetHand());    // new keyword allows for "deep copy" since AbstractCard appears to be a primitive type... somehow

        int actionBudget = this.curAP;
        int iterations = 0;
        while (actionBudget > 0 && currentHand.Count > 0 && iterations < 20){
            iterations++;   // prevent infinite loop
            int rand = Random.Range(0, currentHand.Count);      // For now, just choose random cards
            
            AbstractCard selectedCard = currentHand[rand];
            if (selectedCard.COST > actionBudget){      // if it costs more than we can afford, don't use this card
                continue;
            }

            if (selectedCard.IsAttack()){
                CalculateAttackIntentTarget(selectedCard);
            } else if (selectedCard.IsSkill()){
                CalculateSkillIntentTarget(selectedCard);
            } else if (selectedCard.IsTrait()){
                intents.Add(new EnemyIntent(selectedCard, this.GetCoreArgument()));    // Traits don't need to worry about targeting, really - just use core argument as target
            } else {
                intents.Add(new EnemyIntent(selectedCard, this.GetCoreArgument()));    // This is stuff like Status cards that don't affect arguments themselves so no target is really necessary, but I don't want weird stuff happening, soo... just target core
            }
            actionBudget -= selectedCard.COST;
            currentHand.RemoveAt(rand);
        }
        Debug.Log("Intents generated; generated " + intents.Count + " intents");
    }

    // To be used whenever an argument is destroyed so that intents which were targeting the destroyed argument re-select a target
    public void RecalculateIntents(AbstractArgument destroyedArg){
        List<EnemyIntent> intentsNeedingRetargeting = new List<EnemyIntent>(this.intents).FindAll(intent => intent.argumentTargeted == destroyedArg);   // get a list of intents that were targeting the destroyed arg
        this.intents.RemoveAll(intent => intent.argumentTargeted == destroyedArg);  // remove all intents from the current intent list
        foreach(EnemyIntent retarget in intentsNeedingRetargeting){
            if (retarget.cardToPlay.IsAttack()){
                CalculateAttackIntentTarget(retarget.cardToPlay);
            } else if (retarget.cardToPlay.IsSkill()){
                CalculateSkillIntentTarget(retarget.cardToPlay);
            }
            // don't need to recalculate for trait/status cards since those only ever target the core argument, and if that gets destroyed, no intent recalc necessary
        }
    }

    // protected so that random classes don't access this but i want the ability for certain units (aka bosses) to override attack intent calculations with some other AI
    protected virtual void CalculateAttackIntentTarget(AbstractCard attackCard){
        AbstractCharacter opponent = TurnManager.Instance.GetOtherCharacter(this);
        List<AbstractArgument> possibleTargets = opponent.GetTargetableArguments(true);
        possibleTargets.RemoveAll(target => target.IsPlanted());      // enemies will never target planted arguments with attacks (why would they try and destroy their own planted arguments?)
        foreach(AbstractArgument target in possibleTargets){
            if (target.isPriorityTarget){           // Check for and target priority arguments first
                intents.Add(new EnemyIntent(attackCard, target) );
                break;
            }
        }
        intents.Add(new EnemyIntent(attackCard, possibleTargets[Random.Range(0, possibleTargets.Count)]));        // If no decoy/taunt arguments are found; target a random enemy argument
    }

    protected virtual void CalculateSkillIntentTarget(AbstractCard skillCard){
        List<AbstractArgument> possibleTargets = this.GetTargetableArguments(true);
        intents.Add(new EnemyIntent(skillCard, possibleTargets[Random.Range(0, possibleTargets.Count)]));        // Target a random friendly argument
    }
}