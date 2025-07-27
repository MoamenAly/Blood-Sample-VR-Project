using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[DefaultExecutionOrder(-10000)]
public class MirrorUIContent : MonoBehaviour
{
    public bool mirrorHorizontally = true;
    public bool mirrorVertically = false;


    private void OnEnable()
    {
      //  OnLangueUpdated(LanguageManager.Instance.ActiveLanguage.LanguageLayout);
    }


    void Start()
    {
      //  LanguageManager.Instance.OnLangugaesUpdated += OnLangueUpdated;
    }

    //private void OnLangueUpdated(LanguageLayout languageLayout)
    //{
    //    if(languageLayout==LanguageLayout.RightToLeft)
    //       Mirrior();
    //    else
    //        MirriorN();


    //}

    void Mirrior() {
        TextMeshProUGUI[] childTransforms = GetComponentsInChildren<TextMeshProUGUI>(true);       

        for (int i = 0; i < childTransforms.Length; i++)
        {
            Vector2 size = childTransforms[i].rectTransform.rect.size;
            //Debug.Log(size);
            Vector3 currentPosition = childTransforms[i].rectTransform.localPosition;

            // Set anchorMin and anchorMax to center (0.5, 0.5)
            childTransforms[i].rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            childTransforms[i].rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            // Set pivot to center (0.5, 0.5)
            childTransforms[i].rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Restore the original position
            childTransforms[i].rectTransform.localPosition = currentPosition;

            var scale = childTransforms[i].transform.localScale;
            if (mirrorHorizontally)
            {
                childTransforms[i].transform.localScale = new Vector3(-1 * Mathf.Abs(scale.x), scale.y, scale.z);
                if (childTransforms[i].horizontalAlignment == TMPro.HorizontalAlignmentOptions.Left)
                {
                    Debug.Log("Switched");
                    childTransforms[i].horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
                }
            }
            else
            {
                childTransforms[i].transform.localScale = new Vector3(1 * Mathf.Abs(scale.x), scale.y, scale.z);
                if (childTransforms[i].horizontalAlignment == TMPro.HorizontalAlignmentOptions.Right)
                {
                    Debug.Log("Switched");
                    childTransforms[i].horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                }
            }


            childTransforms[i].rectTransform.sizeDelta = size;

        }


        var parenyScale = transform.localScale;
        if (mirrorHorizontally)
        {
            transform.localScale = new Vector3(-1 * Mathf.Abs(parenyScale.x), parenyScale.y, parenyScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1 * Mathf.Abs(parenyScale.x), parenyScale.y, parenyScale.z);
        }
    }

    void MirriorN()
    {
        TextMeshProUGUI[] childTransforms = GetComponentsInChildren<TextMeshProUGUI>(true);

        for (int i = 0; i < childTransforms.Length; i++)
        {
            Vector2 size = childTransforms[i].rectTransform.rect.size;
            Debug.Log(size);
            Vector3 currentPosition = childTransforms[i].rectTransform.localPosition;

            // Set anchorMin and anchorMax to center (0.5, 0.5)
            childTransforms[i].rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            childTransforms[i].rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            // Set pivot to center (0.5, 0.5)
            childTransforms[i].rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Restore the original position
            childTransforms[i].rectTransform.localPosition = currentPosition;

            var scale = childTransforms[i].transform.localScale;
            if (mirrorHorizontally)
            {

                childTransforms[i].transform.localScale = new Vector3(1 * Mathf.Abs(scale.x), scale.y, scale.z);
                if (childTransforms[i].horizontalAlignment == TMPro.HorizontalAlignmentOptions.Right)
                {
                    Debug.Log("Switched to left");
                    childTransforms[i].horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                }
            }
            else
            {
                childTransforms[i].transform.localScale = new Vector3(1 * Mathf.Abs(scale.x), scale.y, scale.z);
                if (childTransforms[i].horizontalAlignment == TMPro.HorizontalAlignmentOptions.Right)
                {
                    Debug.Log("Switched");
                    childTransforms[i].horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                }
            }


            childTransforms[i].rectTransform.sizeDelta = size;

        }


        var parenyScale = transform.localScale;
        if (mirrorHorizontally)
        {
            transform.localScale = new Vector3(1 * Mathf.Abs(parenyScale.x), parenyScale.y, parenyScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1 * Mathf.Abs(parenyScale.x), parenyScale.y, parenyScale.z);
        }
    }

}
