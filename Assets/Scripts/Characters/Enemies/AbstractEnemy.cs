using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using GameEvent;


public abstract class AbstractEnemy : AbstractCharacter{
    public abstract List<(AbstractCard, AbstractArgument)> CalculateCardsToPlay();
}