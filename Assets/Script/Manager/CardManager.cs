﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
public class CardManager : MonoBehaviour
{
    static CardManager instance;
    public static CardManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("Cardmanager", typeof(CardManager)).GetComponent<CardManager>();
                instance.Init();
            }
            return instance;
        }
    }
    public List<Card> row1,row2,row3;
    public List<Card> cards;
    Deck deckMap = new Deck();

    public Card[] row_1, row_2, row_3;
    public void Init()
    {
        row1 = new List<Card>();
        row2 = new List<Card>();
        row3 = new List<Card>();
        cards = new List<Card>();
    }
    public void SwapCard(int fromIndex, int toIndex) {

        CT_RequestDeckUpdate requestDeckupdate = new CT_RequestDeckUpdate();
        byte[] cardData = new byte[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            cardData[i] = (byte)cards[i].cardID;
        }
        requestDeckupdate.cards = cardData;
        Debug.Log("Send Swap request form "+fromIndex + "to "+toIndex);
        requestDeckupdate.swapCard = new byte[2]{(byte)fromIndex,(byte)toIndex};
        PhotonMessage.RequestSwapCard(new byte[2]{(byte)fromIndex,(byte)toIndex});
    }
    public void UpdateCards(CT_PlayerDeckUpdate deckUpdate){
        ClearCard();
        byte[] cardData = new byte[deckUpdate.cards.Length];
        var swapCard = deckUpdate.swapCard;
        Debug.Log(string.Format("Go Swap Form {0} to {1}",swapCard[0],swapCard[1]));
        var temp = cards[swapCard[0]];
        cards[swapCard[0]] = cards[swapCard[1]];
        cards[swapCard[1]] = temp;
        for (int i = 0; i < deckUpdate.cards.Length; i++)
        {
            cardData[i] = (byte)cards[i].cardID;
            AddToRow(deckMap.GetCardByID(cardData[i]));
        }
    }
    public void SetCardRow()
    {
        row_1 = new Card[2];
        row_2 = new Card[3];
        row_3 = new Card[3];

        cards.CopyTo(0, row_1,0,2);
        cards.CopyTo(2, row_2, 0, 3);
        cards.CopyTo(5, row_3, 0, 3);
        CheckCardRank(row_2);
    }
    public void ClearCard(){
        row1 = new List<Card>();
        row2 = new List<Card>();
        row3 = new List<Card>();
    }

    void CheckCardRank(Card[] cards)
    {
        cards = cards.OrderBy(card => card.cardvalue).ToArray();
        int point = 0;
        bool isStraight = true;
        bool isGhost = cards.All(c => c.cardvalue > 10);
        bool isThreeofKind = cards.All(c => c.cardvalue == cards[0].cardvalue);
        List<bool> suitFlush = new List<bool>();
        suitFlush.Add(cards.All(card => card.GetSuit() == Suits.Clubs));
        suitFlush.Add(cards.All(card => card.GetSuit() == Suits.Diamonds));
        suitFlush.Add(cards.All(card => card.GetSuit() == Suits.Hearts));
        suitFlush.Add(cards.All(card => card.GetSuit() == Suits.Spades));
       
        int currentValue = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            point += (cards[i].GetValue() > 10) ? 10 : cards[i].GetValue();
            
            if(i > 0 && isStraight)
                isStraight = ((cards[i].GetValue() - currentValue) == 1 || (cards[i].GetValue() - currentValue) == 11);
            currentValue = cards[i].GetValue();
        }
        bool getFlush = suitFlush.Any(x => x == true);
        Suits suit = Suits.NONE;
        if (getFlush)
            suit = (Suits)suitFlush.IndexOf(true);
        point = point % 10;
        /*Debug.Log("Getpoint " + point+ "isflush "+getFlush+"suit "+suit);
        Debug.Log("isStraight " + isStraight+ " isGost "+isGhost);
        Debug.Log("isThreeofKind " + isThreeofKind);*/
    }
    public void AddToRow(Card card)
    {
        cards.Add(card);
        if (row1.Count < 2)
        {
            row1.Add(card);
            return;
        }
        if(row2.Count < 3)
        {
            row2.Add(card);
            return;
        }
        if(row3.Count < 3)
        {
            row3.Add(card);
            return;
        }
    }
}
