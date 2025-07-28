using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class LocalizationManager : MonoBehaviour
{
    private static LocalizationManager instance;

    public static LocalizationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LocalizationManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("LocalizationManager");
                    instance = obj.AddComponent<LocalizationManager>();
                }
            }
            return instance;
        }
    }

    private bool _active = false;

    public UnityAction<Locale> OnLocalChangedEvent;
    public UnityEvent OnArabicLanguageSelected, OnEnglishLanguageSelected;

    [Header("UI Controls")]
    public bool isUseDropdown;

    [ShowIf("isUseDropdown")]
    [SerializeField] private TMP_Dropdown languageDropdown;

    public bool isUseButtons;

    [ShowIf("isUseButtons")]
    [SerializeField, ReadOnly] private Button2 englishButton;

    [ShowIf("isUseButtons")]
    [SerializeField, ReadOnly] private Button2 arabicButton;

    public Locale CurrentLocale => LocalizationSettings.SelectedLocale;

    /*
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    */

    private void Start()
    {
        if (isUseDropdown)
        {
            languageDropdown.onValueChanged.AddListener(ChangeLocale);
            int savedID = PlayerPrefs.GetInt("LocalKey", 0);
            languageDropdown.value = savedID;
            ChangeLocale(savedID);
        }

        if (isUseButtons)
        {
            englishButton.onClick.AddListener(EnglishLanguage);
            arabicButton.onClick.AddListener(ArabicLanguage);

            int savedID = PlayerPrefs.GetInt("LocalKey", 0);
            StartCoroutine(SetLocale(savedID));
        }
    }

    [Button]
    [ShowIf("isUseDropdown")]
    public void ChangeLocale(int localeID)
    {
        if (!_active)
            StartCoroutine(SetLocale(localeID));
    }

    [Button]
    [ShowIf("isUseButtons")]
    public void EnglishLanguage()
    {
        if (!_active)
            StartCoroutine(SetLocale(1));
    }

    [Button]
    [ShowIf("isUseButtons")]
    public void ArabicLanguage()
    {
        if (!_active)
            StartCoroutine(SetLocale(0));
    }

    private IEnumerator SetLocale(int localeID)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocalKey", localeID);
        OnLocalChangedEvent?.Invoke(LocalizationSettings.SelectedLocale);
        HandleLanguageLogic();
        _active = false;
    }

    private void HandleLanguageLogic()
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
}
