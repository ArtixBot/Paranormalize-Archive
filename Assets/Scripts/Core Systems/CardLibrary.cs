using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

// CardLibrary pairs card IDs with the actual Class Type. Singleton.
public class CardLibrary
{
    public static readonly CardLibrary Instance = new CardLibrary();
    private Dictionary<string, Type> table = new Dictionary<string, Type>();
    private Dictionary<string, List<AbstractCard>> charTable = new Dictionary<string, List<AbstractCard>>();

    private CardLibrary(){
        float startUp = Time.realtimeSinceStartup;
        Assembly assembly = typeof(AbstractCard).Assembly;

        // Use reflection to grab all of the subclasses of AbstractCard.
        // Definitely eats performance. But this *should* only be called once at the start of the game?
        Type[] cardClasses = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AbstractCard))).ToArray();        // Could also use t.baseClass == typeof(AbstractCard)
        foreach (Type card in cardClasses){
            // For each card that inherits from AbstractCard, create one instance of it to grab the ID so we can form an ID -> class pairing.
            AbstractCard instance = Activator.CreateInstance(card) as AbstractCard;
            table.Add(instance.ID, card);
            
            if (charTable.ContainsKey(instance.DRAFT_CHARACTER)){
                charTable[instance.DRAFT_CHARACTER].Add(instance);
            } else {
                charTable.Add(instance.DRAFT_CHARACTER, new List<AbstractCard>());
                charTable[instance.DRAFT_CHARACTER].Add(instance);
            }
        }
        float endStartUp = Time.realtimeSinceStartup;
        Debug.Log("CardLibrary loaded definition of ALL cards; took " + (endStartUp - startUp) + " seconds.");
    }

    // Given a card's ID, return the appropriate class for that card.
    public Type Lookup(string key){
        try {
            return table[key];
        } catch (KeyNotFoundException){
            Debug.LogError("Key '" + key + "' was not found in CardLibrary!");
            return null;
        }
    }

    // Given an abstract character, return ALL cards that belong to that class. If onlyReturnDraftableByPartner is true, return only cards in the current character's pool that can be drafted by other characters.
    public List<AbstractCard> Lookup(AbstractCharacter character, bool onlyReturnDraftableByPartner = false){
        string key = character.ID;
        try {
            return charTable[key];
        } catch (KeyNotFoundException){
            Debug.LogError("Key '" + key + "' was not found in CardLibrary!");
            return null;
        }
    }
}