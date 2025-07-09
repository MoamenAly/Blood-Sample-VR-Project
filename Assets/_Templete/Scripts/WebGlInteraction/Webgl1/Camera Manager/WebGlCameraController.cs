using BNG;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGlCameraController : MonoBehaviour
{
    [SerializeField] Transform firstMove;
    [SerializeField] Transform secondMove;
    ScreenFader screenFader;

    private void Start()
    {
        screenFader = GetComponent<ScreenFader>();
    }

    public void FirstMove() {

        Debug.Log("first move");
        Move(firstMove);

    }

   public void SecondMove()
   {
        Debug.Log("second move");
        Move(secondMove);
   }

    private void Move(Transform target)
    {

        Debug.Log("first move");

        DOVirtual.DelayedCall(0.5f, () => {
            screenFader.DoFadeIn();

            DOVirtual.DelayedCall(1, () => {
                transform.position = target.position;
                transform.rotation = target.rotation;
                screenFader.DoFadeOut();
            });

        });

       

    }







}
