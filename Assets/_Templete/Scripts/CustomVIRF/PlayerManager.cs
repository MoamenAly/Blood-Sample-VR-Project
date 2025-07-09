using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//ctrl+2R
public class PlayerManager : MonoBehaviour
{

    PlayerRotation playerRotation;
    PlayerTeleport playerTeleport;
    SmoothLocomotion smoothLocomotion;
    HandModelSelector handModelSelector;
    ScreenFader screenFader;
    [FoldoutGroup("VR Player")]
    [SerializeField] HandController leftHand;
    [FoldoutGroup("VR Player")]
    [SerializeField] HandController rightHand;
    GameObject GrabLeft, GrabRight, RemoteGrabLeft, RemoteGrabRight, UIPointerLeft, UIPointerRight;
    [FoldoutGroup("VR Player")]
    [Space][SerializeField] private List<VRPlayerState> vrPlayerStates = new List<VRPlayerState>();
    [FoldoutGroup("VR Player", GroupName = "Defult state")]
    [HideLabel][SerializeField, ReadOnly] private VRPlayerState vrPlayerState;
    [FoldoutGroup("VR Player")]
    [SerializeField] int defultStateIndex = 0;
    private CharacterController _cc;

    private BNGPlayerController _playerController;


    [FoldoutGroup("WEGL Setup")]
    [SerializeField] FreeCamera freeCamera;
    [SerializeField] float _angleRotaion = 20;
    [SerializeField] float refrecePointUpOffest = 0.85f;
    [SerializeField] float cameraDistacne = -0.8f;

    private void OnValidate()
    {
        vrPlayerState.name = "Active Player State";
    }

    private void Awake()
    {
        if (playerRotation == null)
        {
            playerRotation = GetComponentInChildren<PlayerRotation>();
        }

        if (playerTeleport == null)
        {
            playerTeleport = GetComponentInChildren<PlayerTeleport>();
        }

        if (smoothLocomotion == null)
        {
            smoothLocomotion = GetComponentInChildren<SmoothLocomotion>();
        }

        if (screenFader == null)
        {
            screenFader = GetComponent<ScreenFader>();
        }

        if (rightHand != null)
        {
            GrabRight = rightHand.GetComponentInChildren<Grabber>().gameObject;
            RemoteGrabRight = rightHand.GetComponentInChildren<RemoteGrabber>().gameObject;
            UIPointerRight = rightHand.GetComponentInChildren<UIPointer>(true).gameObject;
        }

        if (leftHand != null)
        {
            GrabLeft = leftHand.GetComponentInChildren<Grabber>().gameObject;
            RemoteGrabLeft = leftHand.GetComponentInChildren<RemoteGrabber>().gameObject;
            UIPointerLeft = leftHand.GetComponentInChildren<UIPointer>(true).gameObject;
        }

        _cc = GetComponentInChildren<CharacterController>(true);
        _playerController = GetComponentInChildren<BNGPlayerController>(true);
        handModelSelector = GetComponentInChildren<HandModelSelector>();
    }

    private void Start()
    {
        ActiveVRPlayerState(defultStateIndex);
    }

    public void RestToTransform(Transform refrence)
    {
       // Debug.Log("hello here");
        _cc.enabled = false;
        DOVirtual.DelayedCall(0.5f, () =>
        {
            screenFader.DoFadeIn();
            DOVirtual.DelayedCall(1, () =>
            {

                //update vr player
                _cc.transform.position = refrence.position;
                _cc.transform.rotation = refrence.rotation;
                _cc.enabled = true;

                ////update web camera
                //freeCamera.transform.position = refrence.position + Vector3.up * refrecePointUpOffest;
                //freeCamera.transform.eulerAngles = new Vector3(refrence.rotation.eulerAngles.x + _angleRotaion, refrence.rotation.eulerAngles.y, refrence.rotation.eulerAngles.z);//.rotation;
                //freeCamera.transform.position += freeCamera.transform.forward * cameraDistacne;

                //fade out
                screenFader.DoFadeOut();
            });

        });
    }

    public void ActiveVRPlayerState(int index)
    {
        if (!_cc.gameObject.activeInHierarchy) return;

        vrPlayerState = vrPlayerStates[index];
        ApplayState(vrPlayerState);
    }

    public void ActiveHandModle(int handIndex)
    {
        handModelSelector.ChangeHandsModel(handIndex);
    }

    public void SetPlayerSimulationMode(int vR_Simulation_Mode)
    {

        switch (vR_Simulation_Mode)
        {
            case (int)VR_Simulation_Mode.Stand:
                _playerController.CharacterControllerYOffset = -0.025f;
                break;
            case (int)VR_Simulation_Mode.Seat:
                _playerController.CharacterControllerYOffset = 0.27f;
                break;
        }
    }

    #region Private
    private void ApplayState(VRPlayerState playerComonSetting)
    {
        EnableLocoMotion(playerComonSetting.Locomotion);
        EnableTeleportation(playerComonSetting.teleport);
        EnableRotation(playerComonSetting.Rotation);

        EnableLeftHandUIPointer(playerComonSetting.UIPointer.Left);
        EnableRightHandUIPointer(playerComonSetting.UIPointer.Right);
        EnableLeftHandGrab(playerComonSetting.Grab.Left);
        EnableRightHandGrab(playerComonSetting.Grab.Right);
        EnableRightHandRemoteGrab(playerComonSetting.RemoteGrab.Right);
        EnableLeftHandRemoteGrab(playerComonSetting.RemoteGrab.Left);
    }
    private void EnableLocoMotion(bool status)
    {
        if (status)
        {
            smoothLocomotion.EnableMovement();
        }
        else
        {
            smoothLocomotion.DisableMovement();
        }
    }
    private void EnableTeleportation(bool status)
    {
        if (status)
        {
            playerTeleport.EnableTeleportation();
        }
        else
        {
            playerTeleport.DisableTeleportation();
        }
    }
    private void EnableRotation(bool status)
    {
        playerRotation.AllowInput = status;
    }
    private void EnableLeftHandUIPointer(bool status)
    {
        UIPointerLeft.SetActive(status);
    }
    private void EnableRightHandUIPointer(bool status)
    {
        UIPointerRight.SetActive(status);
    }
    private void EnableLeftHandGrab(bool status)
    {
        GrabLeft.SetActive(status);
    }
    private void EnableRightHandGrab(bool status)
    {
        GrabRight.SetActive(status);
    }
    private void EnableLeftHandRemoteGrab(bool status)
    {
        RemoteGrabLeft.SetActive(status);
    }
    private void EnableRightHandRemoteGrab(bool status)
    {
        RemoteGrabRight.SetActive(status);
    }
    #endregion
}

public enum VR_Simulation_Mode
{
    Stand = 0, Seat = 1, None = 2
}