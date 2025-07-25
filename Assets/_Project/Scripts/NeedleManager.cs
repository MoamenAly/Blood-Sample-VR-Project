using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class NeedleManager : MonoBehaviour
{
    [SerializeField] SnapZone needleSnapZone; 
    [SerializeField] Grabbable vacutainerNeedleGrabbable;
    [SerializeField] Grabbable butterflyNeedleGrabbable;
    [SerializeField] GameObject vacutainerNeedleSnapped;
    [SerializeField] GameObject butterflyNeedleSnapped;
    [SerializeField] UnityEvent2 onCorrectVacutainerNeedle;
    [SerializeField] UnityEvent2 onCorrectButterflyNeedle;
    [SerializeField] UnityEvent2 onWrongNeedle;
    [SerializeField] UnityEvent2 onResetToAgain;
    [SerializeField] float delayToAgain = 4f;

    [SerializeField] ChooseNeedleType chooseNeedleType;

    public void CheckCorrectNeedle()
    {
        if (chooseNeedleType.needleType == NeedleType.Vacutainer && needleSnapZone.HeldItem == vacutainerNeedleGrabbable)
        {
            onCorrectVacutainerNeedle?.Invoke();
            ShowObjectSnapped();
        }
        else if (chooseNeedleType.needleType == NeedleType.Butterfly && needleSnapZone.HeldItem == butterflyNeedleGrabbable)
        {
            onCorrectButterflyNeedle?.Invoke();
            ShowObjectSnapped();
        }
        else
        {
            onWrongNeedle?.Invoke();
            ShowObjectSnapped();
            DOVirtual.DelayedCall(delayToAgain, ResetToAgain);
        }
    }

    public void ResetToAgain()
    {
        needleSnapZone.ReleaseAll();
        needleSnapZone.HeldItem = null;
        vacutainerNeedleGrabbable.gameObject.SetActive(true);
        vacutainerNeedleSnapped.SetActive(false);
        butterflyNeedleGrabbable.gameObject.SetActive(true);
        butterflyNeedleSnapped.SetActive(false);
        onResetToAgain?.Invoke();
    }

    public void ShowObjectSnapped()
    {
        if (needleSnapZone.HeldItem == vacutainerNeedleGrabbable)
        {
            vacutainerNeedleGrabbable.gameObject.SetActive(false);
            vacutainerNeedleSnapped.SetActive(true);
        }
        
        else if (needleSnapZone.HeldItem == butterflyNeedleGrabbable)
        {
            butterflyNeedleGrabbable.gameObject.SetActive(false);
            butterflyNeedleSnapped.SetActive(true);
        }
    }
}
