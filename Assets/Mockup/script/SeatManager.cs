using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SeatManager : MonoBehaviourPun
{
    static SeatManager instance;
    List<Seat> seats;
    public static SeatManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("SeatManager", typeof(SeatManager)).GetComponent<SeatManager>();
                instance.Init();
            }
            return instance;
        }
    }
    void Init()
    {
        seats = new List<Seat>();
    }
    public void RegisterSeat(Seat seat)
    {
        seats.Add(seat);
    }
    public void AcceptSit(AcceptSit sit)
    {
        var seatTarget = seats.Find(s => s.viewID == sit.id);
        if (seatTarget != null)
            seatTarget.Occupied(sit);
    }
    public void ReadyAccept(ReadyAccept accept)
    {
        var seatTarget = seats.Find(s => s.viewID == accept.viewId);
        if(seatTarget != null)
        {
            seatTarget.Ready();
        }
    }
    public void SetSeatData(Dictionary<object,object> seatData)
    {

    }
}



