using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AcceptSit
{
    public int actorNumber;
    public byte id;
    public Player player;
}
public struct ReadyAccept
{
    public int actorNumber;
    public byte viewId;
    public Player player;
}