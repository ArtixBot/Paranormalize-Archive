using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles negotiation. Also stores negotiation state variables, like current round, how many cards were played during this turn, etc.
public class NegotiationManager
{
    public static readonly NegotiationManager Instance = new NegotiationManager();
    // public List<AbstractAction> actionQueue = new List<AbstractAction>();
    public Ambience ambience = Ambience.Instance;
    public TurnManager tm = TurnManager.Instance;

    public AbstractCharacter player;
    public AbstractCharacter enemy;

    public int round = 1;
    public int cardsPlayedThisTurn = 0;

    // Cleans up anything remaining from the previous negotiation.
    // Get the player and enemy from the turn manager. Deep-copy their permadecks to their draw pile.
    public void StartNegotiation(){

        // TODO: REMOVE THIS, CURRENTLY FOR TESTING
        player = new PlayerAi();
        enemy = new TestEnemy();
        TurnManager.Instance.AddToTurnList(player);
        TurnManager.Instance.AddToTurnList(enemy);
        // END TODO

        this.round = 1;
        this.cardsPlayedThisTurn = 0;

        ambience.state = AmbienceState.TENSE;
        player = tm.GetPlayer();
        enemy = tm.GetEnemy();
        
        Debug.Log("NegotiationManager.cs: Performing deep copy of deck contents for enemy and player.");
        DeepCopyDeck(player);
        DeepCopyDeck(enemy);

        player.curAP = player.maxAP;
        enemy.curAP = enemy.maxAP;
        
        player.Draw(5);
        enemy.Draw(5);
        Debug.Log("Negotiation begins!");
    }


    public void NextTurn(){
        Debug.Log("NegotiationManager.cs: Ending turn for " + TurnManager.Instance.GetCurrentCharacter().NAME + ".");
        this.cardsPlayedThisTurn = 0;
        TurnManager.Instance.NextCharacter();
    }

    // Game over!
    public void EndNegotiationLost(){
        Cleanup(player);
        Cleanup(enemy);
    }

    // Victory!
    public void EndNegotiationWon(){
        Cleanup(player);
        Cleanup(enemy);
    }

    public bool PlayCard(AbstractCard card, AbstractCharacter source, AbstractArgument target){
        if (card == null) return false;
        try {
            card.Play(source, target);
            cardsPlayedThisTurn += 1;
            return true;
        } catch (Exception ex){
            Debug.LogWarning("Failed to play card, reason: " + ex.Message);
            return false;
        }
    }

    // (Hopefully) deep copy deck contents from permadeck to drawpile, then shuffle it.
    public void DeepCopyDeck(AbstractCharacter character){
        foreach(AbstractCard card in character.permaDeck.ToList()){
            Type cardType = card.GetType();
            object copiedCard = Activator.CreateInstance(cardType);

            // This _should_ be acceptable enough for what I'm doing.
            // https://stackoverflow.com/questions/12263099/function-to-clone-an-arbitrary-object
            foreach(var property in cardType.GetProperties()){
                object originalCardValue = property.GetValue(card);
                property.SetValue(copiedCard, originalCardValue);
            }
            character.GetDrawPile().AddCard(copiedCard as AbstractCard);
        }
        character.GetDrawPile().Shuffle();
    }

    // post-negotiation cleanup helper function
    public void Cleanup(AbstractCharacter character){
        character.GetDrawPile().Clear();
        character.hand.Clear();
        character.GetDiscardPile().Clear();
    }
}