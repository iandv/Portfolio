using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadSystem : MonoBehaviour
{
    public void SaveAllData()
    {
        OEventManager.Instance.Trigger("OnSave");
    }

    public void LoadAllData()
    {
        OEventManager.Instance.Trigger("OnLoad");
        OEventManager.Instance.Trigger("OnPause");
    }
}
