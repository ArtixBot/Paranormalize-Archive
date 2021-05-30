using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using GameEvent;

public enum FactionType {PLAYER, ENEMY};
public abstract class AbstractCharacter
{
    public string NAME;     // Character name
    public FactionType FACTION;
    
    // HP is based on your core argument.
    public AbstractArgument coreArgument;
    public List<AbstractArgument> nonCoreArguments = new List<AbstractArgument>();
    
    // At end of turn, draw 5 + <drawMod>
    public int drawMod = 0;

    // Action points
    public int maxAP = 3, curAP = 3;

    public Deck permaDeck = new Deck();        // The "permanent deck" of a character. At the start of combat, deep-copy the contents of this deck to drawPile, shuffle drawPile, then draw <X> cards from it.

    public List<AbstractCard> hand = new List<AbstractCard>();
    protected Deck drawPile = new Deck();
    protected Deck discardPile = new Deck();
    protected Deck scourPile = new Deck();

    public bool canPlayCards = true;
    public bool canPlayAttacks = true;
    public bool canPlaySkills = true;
    public bool canPlayTraits = true;
    public bool canPlayDialogue = true;
    public bool canPlayAggression = true;
    public bool canPlayInfluence = true;

    public abstract void AddStarterDeck();      // Should be called at the start of character creation (for players) or at start of combat (for enemies.)

    public void Draw(int numOfCards){
        int i = numOfCards;
        while (i > 0){
            if (drawPile.IsEmpty()){
                // Special case where the draw pile and discard pile are both empty and therefore remaining draws cannot occur.
                if (discardPile.IsEmpty()){
                    Debug.Log("No cards in draw or discard pile for " + this.NAME + "; returning early from draw function");
                    return;
                }
                // Draw pile is empty; copy all card references from discard pile to draw pile and shuffle the draw pile, then clear out the discard pile.
                drawPile.deck.AddRange(discardPile.deck);
                drawPile.Shuffle();
                discardPile.deck.Clear();
                Debug.Log(this.NAME + " shuffles their deck.");
            }

            // Actually add the card from the draw pile to your hand if it's not at max size, else it goes straight to discard.
            AbstractCard drawnCard = drawPile.PopTopCard();
            if (hand.Count < 10){
                hand.Add(drawnCard);
                EventSystemManager.Instance.TriggerEvent(new EventCardDrawn(drawnCard, this));
            } else {
                discardPile.AddCard(drawnCard);
            }
            i--;        // Repeat process <numOfCards> times.
        }
        return;
    }

    public void StartTurn(){
        // Debug.Log("Starting turn of " + this.NAME + ".");
        // remove poise from all arguments
        coreArgument.poise = 0;
        foreach (AbstractArgument aa in this.nonCoreArguments){
            aa.poise = 0;
        }
    }

    public void EndTurn(){
        // Debug.Log("Ending turn of " + this.NAME + ".");
        for (int i = this.hand.Count - 1; i >= 0; i--){
            this.discardPile.AddCard(this.hand[i]);  // Add the card to the discard pile first.
            this.hand.RemoveAt(i);                   // Then actually remove it from the hand.
        }
        this.curAP = this.maxAP;
        this.Draw(5);
    }

    /// <summary> Finds and returns the first argument instance IF it exists. </summary>
    public AbstractArgument GetArgument(AbstractArgument argument){
        foreach (AbstractArgument aa in this.nonCoreArguments){
            if (argument.ID == aa.ID){
                return aa;
            }
        }
        return null;
    }

    // Return all non-core arguments.
    public List<AbstractArgument> GetArguments(){
        return this.nonCoreArguments;
    }

    public AbstractArgument GetCoreArgument(){
        return this.coreArgument;
    }

    public List<AbstractCard> GetHand(){
        return this.hand;
    }

    public Deck GetDrawPile(){
        return this.drawPile;
    }

    public Deck GetDiscardPile(){
        return this.discardPile;
    }
    
    public Deck GetScourPile(){
        return this.scourPile;
    }

    public bool AddCardToPermaDeck(string ID, bool isUpgraded = false){
        AbstractCard reference = this.permaDeck.AddCard(ID, isUpgraded);
        if (reference == null){
            return false;
        } else {
            reference.OWNER = this;
            return true;
        }
    }
}