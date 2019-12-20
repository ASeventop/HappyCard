using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_PlayerDeck : MonoBehaviour
{
    public Image[] cards;
    public void ShowCardFormDeck(byte[] _cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].sprite = AssetManager.GetSprite(_cards[i].ToString());
        }
    }
}
