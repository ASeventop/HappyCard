using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Ready_UI ui_ready;
    private void Awake()
    {
        Instance = this;
    }
    public void SetLocalPlayerSit(bool isSit)
    {
        ui_ready.b_ready.gameObject.SetActive(isSit);
    }
}
