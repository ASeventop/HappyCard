using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : SingletonObject<UserManager>
{
    string[] friends;
    public string[] Friends
    {
        get { return friends; }
        set { friends = value; }
    }
    public void SetData()
    {
        Debug.Log("name of "+nameof(Friends));
    }
    public void Init()
    {
        
    }
}
