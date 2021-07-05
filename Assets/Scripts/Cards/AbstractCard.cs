using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {ATTACK, SKILL, TRAIT, STATUS};
public enum CardAmbient {DIALOGUE, AGGRESSION, INFLUENCE};
public enum CardTags {INHERIT, SCOUR, POISE, DEPLOY, PLANT, INFLUENTIAL, DESTROY};   // Determines what tooltips should appear when viewing the card
public enum CardRarity {STARTER = 0, COMMON = 1, UNCOMMON = 2, RARE = 3, UNIQUE = 4};

public abstract class AbstractCard : EventSubscriber {

    // Gameplay
    public string ID;               // Card ID
    public CardType TYPE;           // Card type
    public CardAmbient AMBIENCE;    // Card ambience type
    public CardRarity RARITY;       // Card rarity
    public int COST;                // Card cost
    public AbstractCharacter OWNER; // Card owner (determined during AbstractCharacter.AddCardToPermaDeck)
    public List<CardTags> TAGS = new List<CardTags>();     // Card tags

    // Cosmetic
    public string NAME;             // Card name
    public Sprite IMAGE;            // Card image path
    public string DESC;             // Card description
    public string FLAVOR;           // Flavor text
    public List<string> QUIPS;      // Say something when a card is played

    public bool isUpgraded = false;
    public bool suppressEventCalls = false;     // should only be true when a card invokes NegotiationManager.Instance.SelectCardsFromList

    public AbstractCard(string id, Dictionary<string, string> cardStrings, int cost, CardAmbient ambience, CardRarity rarity, CardType type, List<CardTags> tags = null){
        this.ID = id;
        this.NAME = cardStrings["NAME"];
        this.DESC = cardStrings["DESC"];
        this.COST = cost;
        this.AMBIENCE = ambience;
        this.RARITY = rarity;
        this.TYPE = type;
        if (tags != null){
            foreach (CardTags tag in tags){
                this.TAGS.Add(tag);
            }
        }
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
        if (this.HasTag(CardTags.DESTROY)){         // Destroy card
            OWNER.Destroy(this);
        } else if (this.IsTrait() || this.HasTag(CardTags.SCOUR)){               // Scour stuff
            OWNER.Scour(this);
        } else {
            source.GetHand().Remove(this);
            source.GetDiscardPile().AddCard(this);
        }
    }

    // Should only ever be overridden whenever a card makes a call to NegotiationManager.Instance.SelectCardsFromList.
    public virtual void PlayCardsSelected(List<AbstractCard> selectedCards){}

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

    public bool HasTag(CardTags tag){
        return this.TAGS.Contains(tag);
    }
}