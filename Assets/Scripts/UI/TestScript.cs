using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public AbstractCharacter character;
    // Start is called before the first frame update
    void Start()
    {
        character = new PlayerDeckard();
        Debug.Log(character.NAME + " " + character.GetCoreArgument().DESC);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
