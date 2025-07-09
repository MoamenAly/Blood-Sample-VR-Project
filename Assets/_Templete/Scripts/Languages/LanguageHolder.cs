using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "LanguageHolder/Language", fileName = "Language", order =1)]
public class LanguageHolder : ScriptableObject
{
    public LanguageLayout LanguageLayout;
    public TMP_FontAsset activeFont;
    [TableList(AlwaysExpanded = true), Searchable]
    public LoclizedText []loclizedText;

    
    Dictionary<Language, string> values = new(); 

    [Button()]
    private void Fetch()
    {
        UpdateBackup();


        var enumType = typeof(Language);
        var enumMembers = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
       

        int numOfIds = enumMembers.Length;
       
        LoclizedText[] temp = new LoclizedText[numOfIds];

    

        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = new LoclizedText()
            {
                textId = (Language)Enum.Parse(typeof(Language), enumMembers[i].Name)
            };

            if (values.ContainsKey(temp[i].textId)) {
                temp[i].text = values[temp[i].textId].TrimEnd();
            }
        }

        loclizedText = temp;
        
    }

    //[Button]
    void UpdateBackup()
    {

        values = new Dictionary<Language, string>();
        for (int i = 0; i < loclizedText.Length; i++)
        {
            values.Add(loclizedText[i].textId, loclizedText[i].text);
        }
    }


}  

[System.Serializable]
public class LoclizedText {

    [TableColumnWidth(150)]
    [FormerlySerializedAs("txtId")] public Language textId;
    [TableColumnWidth(500, Resizable = true)]

    [TextArea(5,20)] public string text;
    private string languageName => textId.ToString();

}

[System.Serializable]
public enum LanguageLayout
{ 
  LeftToRight,RightToLeft
}