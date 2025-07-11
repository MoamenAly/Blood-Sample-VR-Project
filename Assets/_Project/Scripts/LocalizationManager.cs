using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizationManager : MonoSinglton<LocalizationManager>
{
    private bool _active = false;
    public UnityAction<Locale> OnLocalChangedEvent;
    public UnityEvent OnArabicLanguageSelected, OnEnglishLanguageSelected;
    public bool isUseDropdown;
    [ShowIf("isUseDropdown")]
    [SerializeField] private TMP_Dropdown languageDropdown;

    public bool isUseButtons;
    [ShowIf("isUseButtons")]
    [SerializeField] private Button englishButton;
    [ShowIf("isUseButtons")]
    [SerializeField] private Button arabicButton;

    public Locale CurrentLocale
    {
        get
        {
            return LocalizationSettings.SelectedLocale;
        }
    }


    private void Start()
    {
        if (isUseDropdown)
            languageDropdown.onValueChanged.AddListener(ChangeLocale);

        if (isUseButtons)
        {
            englishButton.onClick.AddListener(EnglishLangauge);
            arabicButton.onClick.AddListener(ArabicLangauge);
        }
        int ID = PlayerPrefs.GetInt("LocalKey", 0);
        languageDropdown.value = ID;
        ChangeLocale(ID);
    }

    [Button]
    [ShowIf("isUseDropdown")]
    public void ChangeLocale(int localeID)
    {
        if (_active)
        {
            return;
        }

        StartCoroutine(SetLocale(localeID));
    }

    [Button]
    [ShowIf("isUseButtons")]
    public void EnglishLangauge()
    {
        if (_active)
        {
            return;
        }

        StartCoroutine(SetLocale(0));
    }

    [Button]
    [ShowIf("isUseButtons")]
    public void ArabicLangauge()
    {
        if (_active)
        {
            return;
        }

        StartCoroutine(SetLocale(1));
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
}
