using System;
using UnityEngine;

[Serializable]
public class LocalizationData
{
    public SystemLanguage language;

    public LocalizationData(LocalizationManager lManager)
    {
        language = lManager.language;
    }
}
