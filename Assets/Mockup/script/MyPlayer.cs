﻿using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class MyPlayer : MonoBehaviour
{
    static MyPlayer instance;
    public Player localPlayer;
    public ReactiveProperty<bool> isSit = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> isReady = new ReactiveProperty<bool>(false);
    public ReactiveProperty<int> viewID = new ReactiveProperty<int>(-1);
    public static MyPlayer Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("MyPlayer", typeof(MyPlayer)).GetComponent<MyPlayer>();
            }
            return instance;
        }
    }
    public void SitSeat(int _viewID)
    {
        viewID.Value = _viewID;
    }
    public void Ready(bool _isReady)
    {
        isReady.Value = _isReady;
    }
    public void SetLocalPlayer(Player _localPlayer)
    {
        localPlayer = _localPlayer;
    }
}
