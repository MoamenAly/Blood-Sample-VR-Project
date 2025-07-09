using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

public class LanguageManager : MonoSinglton<LanguageManager>
{
    [SerializeField] LanguageHolder activeLanguage;
    
    public Action<LanguageLayout> OnLangugaesUpdated;


    public LanguageHolder ActiveLanguage { get => activeLanguage;private set => activeLanguage = value; }

    internal string GetText(Language txtId)
    {
        var texts = activeLanguage.loclizedText;
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].textId == txtId)
            {
                return texts[i].text;
            }
        }
        return null;
    }

    [Button]
    public void UpdateLanguages() { 
       OnLangugaesUpdated?.Invoke(activeLanguage.LanguageLayout);
    }


    public void UpdateLanguages(LanguageHolder activeLanguage)
    {
        this.activeLanguage = activeLanguage;
        OnLangugaesUpdated?.Invoke(activeLanguage.LanguageLayout);
    }

}
