using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {ATTACK, SKILL, TRAIT, STATUS};
public enum CardAmbient {DIALOGUE, AGGRESSION, INFLUENCE, PARANORMAL, ITEM, STATUS, NONE};
public enum CardTags {INHERIT, SCOUR, POISE, DEPLOY, PLANT, INFLUENTIAL, DESTROY, PIERCING};   // Determines what tooltips should appear when viewing the card
public enum CardRarity {STARTER = 0, COMMON = 1, UNCOMMON = 2, RARE = 3, UNIQUE = 4};

// Determines what targets can be affected by this card.
// ENEMY - This card can target a single enemy argument (and must be dragged onto one enemy argument)
// ALL_ENEMY - This card can target ALL enemy arguments (and can be dragged onto ANY enemy argument or just the playing field in general)
// ALLY - This card can target a single ally argument
// ALL_ALLY - This card can target ALL ally arguments
// CORE_ONLY - Restricts targeting to only allow targeting of core arguments, and throws otherwise. Do NOT combine with SUPPORT_ONLY
// SUPPORT_ONLY - Restricts targeting to only allow targeting of support arguments, and throws otherwise.
// RANDOM - Targeting is randomized. The card can be dragged onto the playing field (without targeting a specific argument)
public enum CardTarget {ENEMY, ALL_ENEMY, ALLY, ALL_ALLY, CORE_ONLY, SUPPORT_ONLY, RANDOM};

public class CardCostMod {
    public int amount;
    public EventType checkForTerminate;     // Whenever the chosen event runs, check whether to terminate this card's cost.
}

public abstract class AbstractCard : EventSubscriber {

    // Gameplay
    public CardType TYPE;           // Card type
    public CardAmbient AMBIENCE;    // Card ambience type
    public CardRarity RARITY;       // Card rarity
    public int COST;                // Card cost
    public bool COSTS_ALL_AP = false;   // If true, playing this card costs all AP. Set the default cost of the card in-code to 0, though.
    public AbstractCharacter OWNER; // Card owner (determined during AbstractCharacter.AddCardToPermaDeck)
    public List<CardTarget> TARGET; // Card targeting
    public List<CardTags> TAGS = new List<CardTags>();     // Card tags

    // public List<CardCostMod> COST_MODS;// List of modifiers to card cost

    // Metadata
    public string INSTANCE_ID;      // The specific instance id for a card
    public readonly string ID;               // Card ID
    public readonly string DRAFT_CHARACTER;  // Which character's drafts can this card appear in?

    // Cosmetic
    public string NAME;             // Card name
    public string IMAGE;            // Card image filepath
    public string DESC;             // Card description
    public string FLAVOR;           // Flavor text
    public List<string> QUIPS;      // Say something when a card is played

    public bool isUpgraded = false;
    public bool suppressEventCalls = false;     // should only be true when a card invokes NegotiationManager.Instance.SelectCardsFromList

    public AbstractCard(string id, Dictionary<string, string> cardStrings, int cost, CardAmbient ambience, CardRarity rarity, CardType type, List<CardTags> tags = null){
        this.ID = id;
        this.NAME = cardStrings["NAME"];
        this.DESC = cardStrings["DESC"];
        this.IMAGE = (this.ID == "DECKARD_DIPLOMACY") ? "Images/CardsDeckard/diplomacy" : "Images/missing";
        this.COST = cost;
        this.AMBIENCE = ambience;
        this.RARITY = rarity;
        this.TYPE = type;
        if (tags != null){
            foreach (CardTags tag in tags){
                this.TAGS.Add(tag);
            }
        }
        this.DRAFT_CHARACTER = this.ID.Split('_')[0];       // Automatically derive the drafting char from the card ID since all card IDs follow the schemata CHARACTER_CARDNAME
    }

    // A bunch of checks to make sure that we can even play the card -- if we're not able to, don't activate card effects.
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
        // if (this.HasTag(CardTags.DESTROY)){         // Destroy card
        //     this.OWNER.Destroy(this);
        // } else if (this.IsTrait() || this.HasTag(CardTags.SCOUR)){               // Scour stuff
        //     this.OWNER.Scour(this);
        // } else {
        //     if (this.OWNER.GetHand().Contains(this)){           // This check is to prevent adding cards from "choice" mechanics from being added to the discard (see: Deckard's Instincts card)
        //         this.OWNER.GetHand().Remove(this);
        //         this.OWNER.GetDiscardPile().AddCard(this);
        //     }
        // }
    }

    // Should only ever be overridden whenever a card makes a call to NegotiationManager.Instance.SelectCardsFromList.
    public virtual void PlayCardsSelected(List<AbstractCard> selectedCards){}

    public virtual void Upgrade(){
        this.isUpgraded = true;
        this.NAME = this.NAME + "+";
        Dictionary<string, string> checkForUpgDesc = LocalizationLibrary.Instance.GetCardStrings(this.ID);      // if a "DESC+" is defined, use that upgraded description instead once a card is ugpraded.
        if (checkForUpgDesc.ContainsKey("DESC+")){
            this.DESC = checkForUpgDesc["DESC+"];
        }
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