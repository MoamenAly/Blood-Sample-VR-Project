using BNG;
using DG.Tweening;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class Menu : MonoBehaviour, IMenu
{
    public float Scale = 1;
    public Menus MenuId { get => menuId; }
    [SerializeField] Menus menuId;

   

    void Start()
    {
       
        //PlatformManager.Instance.HandleCanvas(gameObject);
    }

    private void OnValidate()
    {
       var size =  GetComponent<RectTransform>().sizeDelta;
        //sizeInPixel.x = size.x * Scale ;
        //sizeInPixel.y = size.y * Scale ;
       transform.localScale = Vector3.one * Scale;
    }

    public void Hide()
    {
        
    }

    public void Show()
    {
        
    }

}

public interface IMenu
{
    void Show();
    void Hide();
}

[ExecuteInEditMode]
public abstract class ThemeMenu : MonoBehaviour
{
    [SerializeField] Theme theme;

    protected virtual void Update()
    {
#if UNITY_EDITOR
     //   ConvertUiToTheme();
     //   ApplayThemes();
#endif
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Debug.Log("5");
        //ConvertUiToTheme();
      //  ApplayThemes();
       // EditorUtility.SetDirty(this);
    }


    private void ApplayThemes()
    {
        return;
        //texts
        var texts = GetComponentsInChildren<Theme_Text>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            var copyText = texts[i];
            switch (copyText.ThemeType) {
                case Theme_Text_Type.Title_Text: 
                    copyText.fontSize = theme.TitleTextSize;
                    copyText.color = theme.TitleTextColor;
                    break;
                case Theme_Text_Type.Title_Text1:
                    copyText.fontStyle = TMPro.FontStyles.Bold;
                    copyText.fontSize = theme.TitleTextSize1;
                    copyText.color = theme.TitleTextColor1;
                    break;
                case Theme_Text_Type.Title_Text2:
                    copyText.fontSize = theme.TitleTextSize2;
                    copyText.color = theme.TitleTextColor2;
                    break;
                case Theme_Text_Type.Description_Text:
                    copyText.fontStyle = TMPro.FontStyles.Normal;
                    copyText.fontSize = theme.DescrotionTextSize;
                    copyText.color = theme.DescrotionTextColor;
                    break;
                case Theme_Text_Type.Button_Text:
                    copyText.fontStyle = TMPro.FontStyles.Bold;
                    copyText.fontSize = theme.ButtonsTextSize;
                    copyText.color = theme.ButtonTextColor;
                    break;
            }
        }

        //images
        var images = GetComponentsInChildren<Theme_Image>(true);
        for (int i = 0; i < images.Length; i++)
        {
            var copyComponent = images[i];
            switch (copyComponent.ThemeType)
            {
                case Theme_Image_Type.Background_Image:
                    copyComponent.color = theme.BackGroundColor;
                    copyComponent.sprite = theme.BackImage;
                    break;
                case Theme_Image_Type.Button_Image:
                    copyComponent.color = theme.ButtonColor;
                    copyComponent.sprite = theme.ButtonImage;

                    break;
                case Theme_Image_Type.Border_Image:
                    copyComponent.color = theme.BorderColor;
                    copyComponent.sprite = theme.BorderImage;
                    break;           
                case Theme_Image_Type.None:
                    break;
            }
        }
    }

    void ConvertUiToTheme() {
        //TEXTS
        var texts = GetAllComponentsInChildren<TMPro.TMP_Text>(gameObject);
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] is Theme_Text) continue;
            var textObject = texts[i].gameObject;
            var copyText = texts[i];
            EditorApplication.delayCall += () => {
                string txt = copyText.text;
                var alinment = copyText.alignment;
                DestroyImmediate(copyText);
                var theme_Text = textObject.AddComponent<Theme_Text>();
                if(theme_Text == null) return;
                theme_Text.text = "T_"+txt;
                theme_Text.alignment = alinment;
                theme_Text.ThemeType = Theme_Text_Type.Title_Text;

            };
        }

        //images
        var images = GetAllComponentsInChildren<Image>(gameObject);
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] is Theme_Image) continue;

            var _gameObject = images[i].gameObject;
            var copyComponent = images[i];
            EditorApplication.delayCall += () => {
                DestroyImmediate(copyComponent);
                var theme_Image = _gameObject.AddComponent<Theme_Image>();
                if(theme_Image == null) return;
                theme_Image.ThemeType = Theme_Image_Type.Background_Image;
            };
        }

        //buttons
        var buttons = GetAllComponentsInChildren<Button>(gameObject);
        for (int i = 0; i < buttons.Length; i++)
        {
            var _gameObject = buttons[i].gameObject;
            var copyComponent = buttons[i];

            var theme_Image = _gameObject.GetComponent<Theme_Image>();
            if (theme_Image == null) continue;
#if UNITY_EDITOR
            EditorApplication.delayCall += () => {
                if (_gameObject == null) return;
                var theme_Image = _gameObject.GetComponent<Theme_Image>();
                //button image
                if(theme_Image == null) return;
                theme_Image.ThemeType = theme_Image.ThemeType != Theme_Image_Type.None ? Theme_Image_Type.Button_Image : Theme_Image_Type.None;
               copyComponent.targetGraphic = theme_Image;

                //button text
               var theme_Text = _gameObject.GetComponentInChildren<Theme_Text>(true);
                if (theme_Text)
                {                    
                 theme_Text.ThemeType = Theme_Text_Type.Button_Text;
                }
            };
#endif
        }

    }

#endif

    // Recursive function to get components of type T from all children and their descendants
    public T[] GetAllComponentsInChildren<T>(GameObject gameObject) where T : Component
    {
        // Start with components from the current GameObject
        T[] components = gameObject.GetComponents<T>();

        // Recursively search through all children and their descendants
        foreach (Transform child in gameObject.transform)
        {
            components = components.Concat(GetAllComponentsInChildren<T>(child.gameObject)).ToArray();
        }

        return components;
    }

}