using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    public Dictionary<string, object> parameter;
    public abstract void SetParameter(Dictionary<string, object> _parameter);
    //Oncreated use when this class create
    public abstract void OnCreated();
    public abstract void OnShow();
    public abstract void OnDestroy();
    public void Dispose()
    {
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }
    private void OnEnable()
    {
        OnCreated();
        OnShow();
    }
    private void OnDisable()
    {
        OnDestroy();
    }
}
