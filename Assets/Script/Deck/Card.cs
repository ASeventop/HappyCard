using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    protected Suits suit;
    public int cardvalue;
    public byte cardID;
    public Card(Suits _suit, int cardvalue2, byte id)
    {
        suit = _suit;
        cardvalue = cardvalue2;
        cardID = id;
        ToString();
    }
    public int GetValue()
    {
        return cardvalue;
    }
    public Suits GetSuit()
    {
        return suit;
    }
    public override string ToString()
    {
        return string.Format("{0} of {1} id {2}", cardvalue, suit, cardID);
    }
}
