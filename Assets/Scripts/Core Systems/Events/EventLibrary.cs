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
    CARD_PLAYED,                    // events like DIALOGUE_CARD_PLAYED or AGGRESSION_CARD_PLAYED should also trigger CARD_PLAYED.
    DIALOGUE_CARD_PLAYED,
    AGGRESSION_CARD_PLAYED,
    INFLUENCE_CARD_PLAYED,
    ATTACK_CARD_PLAYED,
    SKILL_CARD_PLAYED,
    TRAIT_CARD_PLAYED
};

public abstract class AbstractEvent
{
    public EventType type;
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
