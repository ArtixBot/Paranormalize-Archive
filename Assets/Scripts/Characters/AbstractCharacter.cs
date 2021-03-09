using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum FactionType {PLAYER, ENEMY};
public abstract class AbstractCharacter
{
    public string NAME;     // Character name
    public FactionType FACTION;
    
    public int drawModifier = 5;        // Default: At end of turn, draw 5 + <drawModifier>.
    
    // HP is based on your core argument.
    public AbstractArgument coreArgument;
    public List<AbstractArgument> nonCoreArguments;

    // Action points
    public int maxAP, curAP;

    public Deck battleDeck = new Deck();        // The "permanent deck" of a character. At the start of combat, deep-copy the contents of this deck to drawPile, shuffle drawPile, then draw <X> cards from it.

    public List<AbstractCard> hand = new List<AbstractCard>();
    protected Deck drawPile = new Deck();
    protected Deck discardPile = new Deck();

    public abstract void AddStarterDeck();      // Should be called at the start of character creation (for players) or at start of combat (for enemies.)

    public void Draw(int numOfCards){
        int i = numOfCards;
        while (i > 0){
            if (drawPile.IsEmpty()){
                // Special case where the draw pile and discard pile are both empty and therefore remaining draws cannot occur.
                if (discardPile.IsEmpty()){
                    return;
                }
                // Draw pile is empty; copy all card references from discard pile to draw pile and shuffle the draw pile, then clear out the discard pile.
                drawPile.deck.AddRange(discardPile.deck);
                drawPile.Shuffle();
                discardPile.deck.Clear();
            }

            // Actually add the card from the draw pile to your hand if it's not at max size, else it goes straight to discard.
            AbstractCard drawnCard = drawPile.PopTopCard();
            if (hand.Count < 10){
                hand.Add(drawnCard);
            } else {
                discardPile.AddCard(drawnCard);
            }
            i--;        // Repeat process <numOfCards> times.
        }
        return;
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
}