using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventCode : byte
{
    RequestSeatData,
    ReceiveSeatData,
    PlayerReady,
    SendCard,
    SitRequest,
    SitAccept,
    ReadyRequest,
    ReadyAccept,
    GameReady,
    DistributeCard,
    UpdatePlayerDeck,
    ConfirmPlayerDeck,
    UpdateRuleDeck,
    UpdateTimer,
    PlayerUpdateDeckEnd,
    UpdateDeckEnd,
    RestartGameTimer,
    GameResult,
    PlayerLeave,
    RequestFullState,
    ReceiveFullState,
    TestData = 254,
    Rejoin = 255
}
public enum ParameterCode : byte
{
    PlayerReady,
    SendCard
}
public enum Suits
{
    Clubs, Diamonds, Hearts, Spades,NONE
}

//cardRAnk
public enum CardRank
{
    Point,Double,Flush,Ghost, Straight, StraightFlush,ThreeofKind
}
public enum SceneIndex : int
{
    Lobby,GamePlay
}
