using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note that CardType and CardRarity enums are defined outside so that any class can use them (hopefully?)
public enum CardType {ATTACK, SKILL, TRAIT};
public enum CardRarity {STARTER = 0, COMMON = 1, UNCOMMON = 2, RARE = 3, UNIQUE = 4};

public abstract class AbstractCard {

    // Gameplay
    public string ID;               // Card ID
    public CardType TYPE;           // Card type
    public CardRarity RARITY;       // Card rarity
    public int COST;                // Card cost

    // Cosmetic
    public string NAME;             // Card name
    public Sprite IMAGE;            // Card image path
    public string DESC;             // Card description
    public string FLAVOR;           // Flavor text
    
    public bool isUpgraded = false;

    public AbstractCard(string id, string cardName, int cost, CardRarity rarity, CardType type){
        this.ID = id;
        this.NAME = cardName;
        this.COST = cost;
        this.RARITY = rarity;
        this.TYPE = type;
    }

    public virtual void Play(AbstractCharacter source, AbstractCharacter target){}

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
}