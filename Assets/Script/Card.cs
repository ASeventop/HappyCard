using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Card
{
    protected Suits suit;
    protected string cardvalue;
    public byte cardID;
    public Card(Suits _suit, string cardvalue2, byte id)
    {
        suit = _suit;
        cardvalue = cardvalue2;
        cardID = id;
    }
    public override string ToString()
    {
        return string.Format("{0} of {1} id {2}", cardvalue, suit, cardID);
    }
}
