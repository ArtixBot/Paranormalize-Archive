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

    public void StartNegotiation(){
        // Place the player in the queue.
        // Add each enemy into the queue.
        // For everyone in queue: deep-copy contents of battledeck into the drawpile, then draw (5 + drawModifier).
        // Add intrinsic conditions to everyone.
        ambience.state = AmbienceState.TENSE;
        player = tm.GetPlayer();
        enemy = tm.GetEnemy();
        
        player.Draw(5);
        enemy.Draw(5);
    }

    public void NextTurn(){
        this.cardsPlayedThisTurn = 0;
        TurnManager.Instance.NextCharacter();
    }

    public void EndNegotiationLost(){
        // Game over!
    }

    public void EndNegotiationWon(){
        // Reset variables to their original state
        this.round = 1;
        this.cardsPlayedThisTurn = 0;
    }

    public bool PlayCard(AbstractCard card, AbstractCharacter source, AbstractCharacter target){
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
}