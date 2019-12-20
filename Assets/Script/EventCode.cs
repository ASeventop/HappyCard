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
    DistributeCard
}
public enum ParameterCode : byte
{
    PlayerReady,
    SendCard
}
public enum Suits
{
    Clubs, Diamonds, Hearts, Spades
}
