using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Themes/theme",fileName ="Theme",order = 1)]
public class Theme : ScriptableObject
{
    [FoldoutGroup("Colors")]
    public Color BackGroundColor = Color.black;
    [FoldoutGroup("Colors")]
    public Color BorderColor = Color.red;
    [FoldoutGroup("Colors")]
    public Color TitleTextColor = Color.white;
    [FoldoutGroup("Colors")]
    public Color DescrotionTextColor = Color.white;
    [FoldoutGroup("Colors")]
    public Color ButtonTextColor = Color.blue;
    [FoldoutGroup("Colors")]
    public Color ButtonColor = Color.white;
    [FoldoutGroup("Colors")]
    public Color TitleTextColor1 = Color.black;
    [FoldoutGroup("Colors")]
    public Color TitleTextColor2 = Color.red;



    [FoldoutGroup("Text Sizes")]
    public Vector2 TitleTextSizeValues = new Vector2(10, 50);


    [FoldoutGroup("Text Sizes")]
    public int TitleTextSize = 50;



    [FoldoutGroup("Text Sizes")]
    public float TitleTextSize1;
    [FoldoutGroup("Text Sizes")]
    public float TitleTextSize2;
    [FoldoutGroup("Text Sizes")]
    public int DescrotionTextSize = 30;
    [FoldoutGroup("Text Sizes")]
    public int ButtonsTextSize = 40;

    [FoldoutGroup("Sprites")]
    public Sprite BackImage;
    [FoldoutGroup("Sprites")]
    public Sprite BorderImage;
    [FoldoutGroup("Sprites")]
    public Sprite ButtonImage;

   

}
