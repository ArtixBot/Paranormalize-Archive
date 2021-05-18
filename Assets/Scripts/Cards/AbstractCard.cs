using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note that CardType and CardRarity enums are defined outside so that any class can use them (hopefully?)
public enum CardType {ATTACK, SKILL, TRAIT};
public enum CardAmbient {DIALOGUE, AGGRESSION, INFLUENCE, STATUS};
public enum CardRarity {STARTER = 0, COMMON = 1, UNCOMMON = 2, RARE = 3, UNIQUE = 4};

public abstract class AbstractCard {

    // Gameplay
    public string ID;               // Card ID
    public CardType TYPE;           // Card type
    public CardAmbient AMBIENCE;    // Card ambience type
    public CardRarity RARITY;       // Card rarity
    public int COST;                // Card cost

    // Cosmetic
    public string NAME;             // Card name
    public Sprite IMAGE;            // Card image path
    public string DESC;             // Card description
    public string FLAVOR;           // Flavor text
    public List<string> QUIPS;      // Say something when a card is played

    public bool isUpgraded = false;

    public AbstractCard(string id, Dictionary<string, string> cardStrings, int cost, CardAmbient ambience, CardRarity rarity, CardType type){
        this.ID = id;
        this.NAME = cardStrings["NAME"];
        this.DESC = cardStrings["DESC"];
        this.COST = cost;
        this.AMBIENCE = ambience;
        this.RARITY = rarity;
        this.TYPE = type;
    }

    public virtual void Play(AbstractCharacter source, AbstractArgument target){
        if (!source.canPlayCards){
            throw new Exception(source.NAME + " cannot play cards!");
        }
        if ((this.IsAttack() && !source.canPlayAttacks) || (this.IsSkill() && !source.canPlaySkills) || (this.IsTrait() && !source.canPlayTraits)){
            throw new Exception(source.NAME + " cannot play card of type " + this.TYPE.ToString());
        }
        if ((this.IsDialogue() && !source.canPlayDialogue) || (this.IsAggression() && !source.canPlayAggression) || (this.IsInfluence() && !source.canPlayInfluence)){
            throw new Exception(source.NAME + " cannot play card of type " + this.AMBIENCE.ToString());
        }
        if (source.curAP < this.COST){
            throw new Exception(source.NAME + " does not have enough actions to play " + this.NAME);
        }
        source.curAP -= this.COST;
        source.GetHand().Remove(this);      // Currently removes the first occurrence of a card instead of the actual card itself...I think. Fix by using ID check instead?
        source.GetDiscardPile().AddCard(this);
    }

    public virtual void Upgrade(){
        this.isUpgraded = true;
        this.NAME = this.NAME + "+";
    }

    public bool IsAttack(){
        return this.TYPE == CardType.ATTACK;
    }

    public bool IsSkill(){
        return this.TYPE == CardType.SKILL;
    }

    public bool IsTrait(){
        return this.TYPE == CardType.TRAIT;
    }

    public bool IsDialogue(){
        return this.AMBIENCE == CardAmbient.DIALOGUE;
    }

    public bool IsAggression(){
        return this.AMBIENCE == CardAmbient.AGGRESSION;
    }
    
    public bool IsInfluence(){
        return this.AMBIENCE == CardAmbient.INFLUENCE;
    }
}