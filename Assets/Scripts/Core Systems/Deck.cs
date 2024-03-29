using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

// At start of combat, create a copy of the player's deck and shuffle the copy.
public class Deck
{    
    // For reference, let index 0 be the topmost card.
    public List<AbstractCard> deck = new List<AbstractCard>();

    // Shuffles the deck.
    public void Shuffle(){
        System.Random rng = new System.Random();
        int n = this.deck.Count;
        while (n > 1){
            n--;
            int k = rng.Next(n + 1);
            AbstractCard card = this.deck[k];

            this.deck[k] = this.deck[n];
            this.deck[n] = card;
        }
    }

    // Returns true if the deck is empty.
    public bool IsEmpty(){ return this.deck.Count == 0; }

    // Add a card to the back of the deck via string ID.
    // By default adds an unupgraded version to the deck, but can be upgraded by setting isUpgraded to true.
    // Returns the created card instance.
    public AbstractCard AddCard(string cardID, bool isUpgraded){
        Type cardClass = CardLibrary.Instance.Lookup(cardID);
        if (cardClass == null){
            return null;        // return a null result if the card doesn't exist in CardLibrary
        }
        AbstractCard card = Activator.CreateInstance(cardClass) as AbstractCard;
        if (isUpgraded){
            card.Upgrade();
        }
        this.deck.Add(card);
        return card;
    }

    // Add a card to the back of the deck via reference. Example usage: When the user discards their hand, all non-retaining cards need to be put into the discard pile, which is classified as a deck.
    public void AddCard(AbstractCard card){
        this.deck.Add(card);
    }

    // Return the first instance of the card (starting from the back of the deck due to collection modification)
    public void RemoveCard(AbstractCard card){
        for (int i = this.deck.Count() - 1; i >= 0; i--){
            if (this.deck[i] == card){
                this.deck.RemoveAt(i);
                break;
            }
        }
    }

    // Find and return a card by INSTANCE_ID value.
    public AbstractCard GetCard(string INSTANCE_ID){
        for (int i = 0; i < this.deck.Count(); i++){
            if (this.deck[i].INSTANCE_ID == INSTANCE_ID){
                return this.deck[i];
            }
        }
        return null;
    }

    // Returns deck size.
    public int GetSize(){
        return this.deck.Count;
    }

    // Returns the top card of the deck and removes it from the list. If NULL is returned, the deck is empty.
    public AbstractCard PopTopCard(){
        if (this.deck.Count != 0){
            AbstractCard card = this.deck[0];
            this.deck.RemoveAt(0);
            return card;
        }
        return null;
    }

    // Returns the entire deck.
    public List<AbstractCard> ToList(){
        return this.deck;
    }

    // Remove all contents from the deck. Called when negotiation ends.
    public void Clear(){
        this.deck.Clear();
    }

    // Returns the top <X> cards of the deck. If <X> exceeds decksize, only <decksize> cards are returned.
    public List<AbstractCard> GetTopXCards(int count){
        return this.deck.Take(count).ToList();  // If we try to get top 20 cards or whatever, but current deck size is 4, then just 4 elements will be returned. If nothing in the deck remains, the list returned is empty.
    }
}
