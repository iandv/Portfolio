using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateLanguage : MonoBehaviour
{
    public string textKey;
    private TextMeshProUGUI _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        OEventManager.Instance.Subscribe("LanguageChange", OnLanguageChange);
        OnLanguageChange();
    }

    void OnLanguageChange(params object[] parameters)
    {
        _textComponent.text = LocalizationManager.Instance.GetText(textKey);
    }
}
