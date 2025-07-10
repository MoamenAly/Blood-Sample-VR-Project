using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Components;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;
public class LocalizationManager : MonoSinglton<LocalizationManager>
{
    private bool _active = false;
    public UnityAction<Locale> OnLocalChangedEvent;
    public UnityEvent OnArabicLanguageSelected, OnEnglishLanguageSelected;
    [SerializeField] private TMP_Dropdown languageDropdown;

    public Locale CurrentLocale
    {
        get
        {
            return LocalizationSettings.SelectedLocale;
        }
    }


    private void Start()
    {
        languageDropdown.onValueChanged.AddListener(ChangeLocale);
        int ID = PlayerPrefs.GetInt("LocalKey", 0);
        languageDropdown.value = ID;
        ChangeLocale(ID);
    }

    public void ChangeLocale(int localeID)
    {
        if (_active)
        {
            return;
        }

        StartCoroutine(SetLocale(localeID));
    }
    private IEnumerator SetLocale(int localeID)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocalKey", localeID);
        OnLocalChangedEvent?.Invoke(LocalizationSettings.SelectedLocale);
        handleLangugeLogic();
        _active = false;
    }

    void handleLangugeLogic()
    {

        if (CurrentLocale.Identifier.Code == "ar")
        {
            OnArabicLanguageSelected?.Invoke();
        }
        else if (CurrentLocale.Identifier.Code == "en")
        {
            OnEnglishLanguageSelected?.Invoke();
        }

    }
    public bool IsRTLLanguageSelected()
    {
        return CurrentLocale.Identifier.Code == "ar";
    }





    [Button]
    public void ChangeToEnglish()
    {
        ChangeLocale(0);

    }


    [Button]
    public void ChangeToArabic()
    {
        ChangeLocale(1);

    }





}
