using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// A list of every triggerable event.

public enum EventType {
    AMBIENCE_STATE_SHIFT,
    ARGUMENT_ATTACKED_BLOCKED,
    ARGUMENT_ATTACKED_UNBLOCKED,
    ARGUMENT_CREATED,
    ARGUMENT_DESTROYED,
    ARGUMENT_STACKS_ADDED,
    ARGUMENT_STATUS_EFFECT_APPLIED,
    CARD_DRAWN,
    CARD_PLAYED,
    TURN_START,
    TURN_END,
    POISE_APPLIED
};

public abstract class AbstractEvent{
    public EventType type;
};

namespace GameEvent{

    public class EventAmbientStateShift : AbstractEvent {
        public AmbienceState oldState;
        public AmbienceState newState;
        ///<summary>
        ///Trigger when the ambience state shifts. Takes in an oldState (the previous ambience) and newState (the new ambience).
        ///</summary>
        public EventAmbientStateShift(AmbienceState oldState, AmbienceState newState){
            this.type = EventType.AMBIENCE_STATE_SHIFT;
            this.oldState = oldState;
            this.newState = newState;
        }
    }

    public class EventCardDrawn : AbstractEvent {
        public AbstractCard cardDrawn;
        public AbstractCharacter owner;
        ///<summary>
        ///Trigger when a card is drawn. Takes two arguments - card, representing the card drawn; and cardOwner, representing the owner of that card.
        ///</summary>
        public EventCardDrawn(AbstractCard card, AbstractCharacter cardOwner){
            this.type = EventType.CARD_DRAWN;
            this.cardDrawn = card;
            this.owner = cardOwner;
        }
    }

    public class EventTurnStart : AbstractEvent{
        public AbstractCharacter start;
        ///<summary>
        ///Trigger when a character's turn starts.
        ///</summary>
        public EventTurnStart(AbstractCharacter character){
            this.type = EventType.TURN_START;
            this.start = character;
        }
    }

    public class EventTurnEnd : AbstractEvent{
        public AbstractCharacter end;
        ///<summary>
        ///Trigger when a character's turn ends.
        ///</summary>
        public EventTurnEnd(AbstractCharacter character){
            this.type = EventType.TURN_END;
            this.end = character;
        }
    }

    public class EventArgAttackedBlocked : AbstractEvent{
        public AbstractArgument argumentAttacked;
        public int damageDealt;
        ///<summary>
        ///Trigger when an argument is attacked but all the damage was blocked. argAttacked represents the argument being attacked and damageDealtToArg represents the damage dealt to the argument.
        ///</summary>
        public EventArgAttackedBlocked(AbstractArgument argAttacked, int damageDealtToArg){
            this.type = EventType.ARGUMENT_ATTACKED_BLOCKED;
            this.argumentAttacked = argAttacked;
            this.damageDealt = damageDealtToArg;
        }
    }

    public class EventArgAttackedUnblocked : AbstractEvent{
        public AbstractArgument argumentAttacked;
        public int damageDealt;
        ///<summary>
        ///Trigger when an argument is attacked but at least one point of damage was unblocked. argAttacked represents the argument being attacked and damageDealtToArg represents the damage dealt to the argument.
        ///</summary>
        public EventArgAttackedUnblocked(AbstractArgument argAttacked, int damageDealtToArg){
            this.type = EventType.ARGUMENT_ATTACKED_UNBLOCKED;
            this.argumentAttacked = argAttacked;
            this.damageDealt = damageDealtToArg;
        }
    }

    public class EventArgCreated : AbstractEvent{
        public AbstractArgument argumentCreated;
        ///<summary>
        ///Trigger when an argument is created. argCreated represents the argument being created.
        ///</summary>
        public EventArgCreated(AbstractArgument argCreated){
            this.type = EventType.ARGUMENT_CREATED;
            this.argumentCreated = argCreated;
        }
    }

    public class EventArgStacksAdded : AbstractEvent{
        public AbstractArgument argumentAddedTo;
        public int stacksAdded;
        public int newStackCount;
        ///<summary>
        ///Trigger when an argument has stacks added to.
        ///</summary>
        public EventArgStacksAdded(AbstractArgument argAddedTo, int stacks){
            this.type = EventType.ARGUMENT_STACKS_ADDED;
            this.argumentAddedTo = argAddedTo;
            this.stacksAdded = stacks;
            this.newStackCount = this.argumentAddedTo.stacks;
        }
    }

    public class EventArgDestroyed : AbstractEvent{
        public AbstractArgument argumentDestroyed;
        public AbstractArgument destroyingArg;
        public AbstractCard destroyingCard;
        ///<summary>
        ///Trigger when an argument is destroyed. argDestroyed represents the argument being destroyed.
        ///</summary>
        public EventArgDestroyed(AbstractArgument argDestroyed){
            this.type = EventType.ARGUMENT_DESTROYED;
            this.argumentDestroyed = argDestroyed;
        }

        public EventArgDestroyed(AbstractArgument argDestroyed, AbstractCard destroyingCard){
            this.type = EventType.ARGUMENT_DESTROYED;
            this.argumentDestroyed = argDestroyed;
            this.destroyingCard = destroyingCard; 
        }

        public EventArgDestroyed(AbstractArgument argDestroyed, AbstractArgument destroyingArg){
            this.type = EventType.ARGUMENT_DESTROYED;
            this.argumentDestroyed = argDestroyed;
            this.destroyingArg = destroyingArg;
        }
    }

    public class EventCardPlayed : AbstractEvent{
        public AbstractCard cardPlayed;
        public AbstractCharacter playedBy;
        public CardType cardType;
        public CardAmbient cardAmbient;
        ///<summary>
        ///Trigger when a card is played. card represents the card being played, player represents the character playing the card.
        ///</summary>
        public EventCardPlayed(AbstractCard card, AbstractCharacter player){
            this.type = EventType.CARD_PLAYED;
            this.playedBy = player;

            this.cardPlayed = card;
            this.cardAmbient = card.AMBIENCE;
            this.cardType = card.TYPE;
        }
    }

    public class EventPoiseApplied : AbstractEvent{
        public int poiseApplied;
        public AbstractArgument target;
        ///<summary>
        ///Trigger whenever Poise is applied. poise is the amount of poise that was applied (post-resolution), and target is the argument which gained the poise.
        ///</summary>
        public EventPoiseApplied(AbstractArgument target, int poiseApplied){
            this.type = EventType.POISE_APPLIED;
            this.poiseApplied = poiseApplied;
            this.target = target;
        }
    }

    public class EventStatusEffectApplied : AbstractEvent{
        public AbstractArgument argAppliedTo;
        public AbstractStatusEffect effectApplied;
        public int stacksAdded;
        ///<summary>
        ///Trigger when an argument has stacks added to.
        ///</summary>
        public EventStatusEffectApplied(AbstractArgument argAppliedTo, AbstractStatusEffect effect, int stacks = 0){
            this.type = EventType.ARGUMENT_STATUS_EFFECT_APPLIED;
            this.argAppliedTo = argAppliedTo;
            this.effectApplied = effect;
            this.stacksAdded = stacks;
        }
    }
}
