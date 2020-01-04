using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck 
{
    Card[] cards = new Card[52];
    int[] numbers = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 1 };
    public Deck()
    {
        byte i = 0;
        foreach (int s in numbers)
        {
            cards[i] = new Card(Suits.Clubs, s, i);
            i++;
        }
        foreach (int s in numbers)
        {
            cards[i] = new Card(Suits.Diamonds, s, i);
            i++;
        }
        foreach (int s in numbers)
        {
            cards[i] = new Card(Suits.Hearts, s, i);
            i++;
        }
        foreach (int s in numbers)
        {
            cards[i] = new Card(Suits.Spades, s, i);
            i++;
        }
    }

    public Card GetCardByID(byte id)
    {
        return cards[id];
    }
}
