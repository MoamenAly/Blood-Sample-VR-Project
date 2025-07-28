using DG.Tweening;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

[DefaultExecutionOrder(-10000)]
public class MirrorUIContent : MonoBehaviour
{
    public bool mirrorHorizontally = true;
    public bool mirrorVertically = false;

    [Header("Font Names")]
    public string arabicFontName = "ArabicFont";
    public string englishFontName = "EnglishFont";

    private TMP_FontAsset arabicFont;
    private TMP_FontAsset englishFont;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;

        // Load fonts from Resources
        arabicFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/" + arabicFontName);
        englishFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/" + englishFontName);

        if (arabicFont == null)
            Debug.LogError($"Arabic font not found at Resources/Fonts/{arabicFontName}");
        if (englishFont == null)
            Debug.LogError($"English font not found at Resources/Fonts/{englishFontName}");
    }

    private void Start()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLanguageUpdated;
        OnLanguageUpdated(LocalizationManager.Instance.CurrentLocale);
    }

    private void OnLanguageUpdated(UnityEngine.Localization.Locale newLocale)
    {
        bool isArabic = newLocale.Identifier.Code.StartsWith("ar");
        ApplyMirrorAndFont(isArabic);
    }

    private void ApplyMirrorAndFont(bool isArabic)
    {
        RTLTextMeshPro[] texts = GetComponentsInChildren<RTLTextMeshPro>(true);

        foreach (var text in texts)
        {
            // Switch font
            text.font = isArabic ? arabicFont : englishFont;

            // Switch alignment only if not centered
            if (text.alignment != TextAlignmentOptions.Center &&
                text.alignment != TextAlignmentOptions.CenterGeoAligned &&
                text.alignment != TextAlignmentOptions.Midline &&
                text.alignment != TextAlignmentOptions.Baseline)
            {
                if (isArabic && text.alignment == TextAlignmentOptions.Left)
                    text.alignment = TextAlignmentOptions.Right;
                else if (!isArabic && text.alignment == TextAlignmentOptions.Right)
                    text.alignment = TextAlignmentOptions.Left;
            }

            // Mirror scale
            var scale = text.transform.localScale;
            text.transform.localScale = new Vector3(
                mirrorHorizontally ? (isArabic ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x)) : scale.x,
                mirrorVertically ? -Mathf.Abs(scale.y) : scale.y,
                scale.z
            );
        }

        // Mirror parent object if needed
        transform.localScale = new Vector3(
            mirrorHorizontally ? (isArabic ? -Mathf.Abs(originalScale.x) : Mathf.Abs(originalScale.x)) : originalScale.x,
            mirrorVertically ? -Mathf.Abs(originalScale.y) : originalScale.y,
            originalScale.z
        );
    }

}
