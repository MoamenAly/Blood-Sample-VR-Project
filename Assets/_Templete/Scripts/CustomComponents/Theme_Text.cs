using RTLTMPro;
using Sirenix.OdinInspector;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Theme_Text : RTLTextMeshPro
{
    [LabelText("Text ID")]
    [SerializeField] public Theme_Text_Type ThemeType;

    public Language TextId;

    private string languageTxt;


    public void UpdateTextBackup() {
        if (languageTxt != TextId.ToString())
            languageTxt = TextId.ToString();
    }

    public void RestoreTextBackup()
    {
        if (languageTxt != TextId.ToString())
            TextId = ParseEnum<Language>(languageTxt);
    }


#if UNITY_EDITOR

  
    protected override void OnValidate()
    {
        if (LanguageManager.Instance == null)
        {
            LanguageManager.Instance = FindObjectOfType<LanguageManager>();
        }

       // RestoreTextBackup();

        UpdateFont(LanguageManager.Instance.ActiveLanguage.LanguageLayout);
    }

    private void LateUpdate()
    {
      //  Debug.Log("updating"); 
      
            if (LanguageManager.Instance == null)
            {
                LanguageManager.Instance = FindObjectOfType<LanguageManager>();
            }

       // UpdateTextBackup();

        UpdateFont(LanguageManager.Instance.ActiveLanguage.LanguageLayout);
        
    }
#endif

    protected override void OnEnable()
    {
        base.OnEnable();
        if (LanguageManager.Instance == null)
        {
            LanguageManager.Instance = FindObjectOfType<LanguageManager>();
        }

       // RestoreTextBackup();


        UpdateFont(LanguageManager.Instance.ActiveLanguage.LanguageLayout);
    }

    protected override void Start()
    {
        
          //  RestoreTextBackup();
        

        base.Start();
        if (LanguageManager.Instance != null) {
            LanguageManager.Instance.OnLangugaesUpdated += UpdateFont;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (LanguageManager.Instance != null) {
            LanguageManager.Instance.OnLangugaesUpdated -= UpdateFont;
        }
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    void UpdateFont(LanguageLayout languageLayout)
    {
        if(LanguageManager.Instance == null)
        {
            LanguageManager.Instance = FindObjectOfType<LanguageManager>();
        }

        if (LanguageManager.Instance == null) return;

        text = LanguageManager.Instance.GetText(TextId);
        
        if (LanguageManager.Instance.ActiveLanguage.activeFont != null)
        {
            font = LanguageManager.Instance.ActiveLanguage.activeFont;
        }
    }

    public void _SetTextID(Language id) {
       TextId = id;
       text = LanguageManager.Instance.GetText(id);
    }
}
