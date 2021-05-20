using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// A list of every triggerable event.

public enum EventType {
    ARGUMENT_ATTACKED_BLOCKED,
    ARGUMENT_ATTACKED_UNBLOCKED,
    ARGUMENT_CREATED,
    ARGUMENT_DESTROYED,
    CARD_PLAYED,
    TURN_START,
    TURN_END
};

public abstract class AbstractEvent
{
    public EventType type;
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

public class EventArgDestroyed : AbstractEvent{
    public AbstractArgument argumentDestroyed;
    ///<summary>
    ///Trigger when an argument is destroyed. argDestroyed represents the argument being destroyed.
    ///</summary>
    public EventArgDestroyed(AbstractArgument argDestroyed){
        this.type = EventType.ARGUMENT_DESTROYED;
        this.argumentDestroyed = argDestroyed;
    }
}

public class EventCardPlayed : AbstractEvent{
    public AbstractCard cardPlayed;
    public AbstractCharacter playedBy;
    public CardType cardType;
    public CardAmbient cardAmbient;
    ///<summary>
    ///Trigger when a card is played. card represents the card being played, ac represents the character playing the card.
    ///</summary>
    public EventCardPlayed(AbstractCard card, AbstractCharacter player){
        this.type = EventType.CARD_PLAYED;
        this.playedBy = player;

        this.cardPlayed = card;
        this.cardAmbient = card.AMBIENCE;
        this.cardType = card.TYPE;
    }
}