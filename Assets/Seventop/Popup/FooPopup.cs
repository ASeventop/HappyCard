using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooPopup : Popup
{
    public static void Launch()
    {
        Debug.Log("Hey Foo");
    }
    public override void OnCreated()
    {
    }

    public override void OnDestroy()
    {
    }

    public override void OnShow()
    {
    }

    public override void SetParameter(Dictionary<string, object> _parameter)
    {
    }
}
