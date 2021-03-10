using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public AbstractCharacter character;
    public AbstractCard card;
    // Start is called before the first frame update
    void Start()
    {
        character = new PlayerDeckard();
        character.AddStarterDeck();
        // Debug.Log("TestScript: " + character.GetDrawPile().GetTopXCards(1)[0].NAME + " " + character.GetDrawPile().GetTopXCards(1)[0].DESC);
    }
}
