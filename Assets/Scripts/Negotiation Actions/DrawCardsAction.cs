using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsAction : AbstractAction {

    private AbstractCharacter source;
    private int cardsToDraw;

    ///<summary>
    ///Draw cards.
    ///<list type="bullet">
    ///<item><term>src</term><description>The character drawing the cards.</description></item>
    ///<item><term>target</term><description>The amount of cards to draw.</description></item>
    ///</list>
    ///</summary>
    public DrawCardsAction(AbstractCharacter src, int cardsToDraw){
        this.source = src;
        this.cardsToDraw = cardsToDraw;
    }

    ///<returns>An integer of how many cards were drawn.</returns>
    public override int Resolve(){
        // AbstractCharacter Draw() implementation handles special cases of max hand size, not being able to draw, etc. automatically, so that work doesn't need to be done here.
        source.Draw(this.cardsToDraw);
        return this.cardsToDraw;
    }
}