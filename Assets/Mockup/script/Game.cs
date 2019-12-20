using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    [SerializeField] GameObject playerDeck_gameObject;
    public UI_PlayerDeck playerDeck;
    private void Awake()
    {
        Instance = this;
    }
    public void OpenPlayerDeck(bool open)
    {
        playerDeck_gameObject.SetActive(open);
    }
}
