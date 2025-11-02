using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class RestOnUnGrab : MonoBehaviour
{
    internal Vector3 defaultPosition;
    Vector3 defaultUp;
    Quaternion defaultRoation;

    Vector3 intialPostion;

    [SerializeField] bool overrideTransform = false;
    [ShowIf(nameof(overrideTransform))][SerializeField] internal Transform overrideedTransform;


    LayerMask OnhandLayer = 1 << 7;
    LayerMask FreeLayer = 1 << 6;

    CustomGrabbable grabbable;
    bool grabed = false;
    bool attempGrab; //in remote grabing
    Rigidbody _rigidbody;

    public UnityEvent onGrab;
    public UnityEvent onUngrab;

    public ResetState ResetState;


    [SerializeField] bool shouldRest = false;
    [ShowIf(nameof(shouldRest))]

   private ChooseNeedleType chooseNeedleType;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.angularDrag = 1;
        _rigidbody.drag = 1;

        defaultPosition = transform.localPosition;
        defaultRoation = transform.localRotation;
    }


    private void Start()
    {
        defaultPosition = transform.position;
        defaultRoation = transform.rotation;
        defaultUp = transform.up;

        intialPostion = transform.position;

        grabbable = GetComponent<CustomGrabbable>();

        if (grabbable != null)
        {
            grabbable.OnReleaseAction += OnUnGrab;
            grabbable.OnGrabAction += OnGrab;
        }

        //gameObject.layer = LayerMaskToLayer(FreeLayer);

    }

    private void OnGrab(Grabber grabber)
    {
        grabed = true;
        onGrab.Invoke();
    }

    private void OnUnGrab()
    {
        onUngrab?.Invoke();
        CancelInvoke(nameof(BackToDefaultPostion));
        grabed = false;
        Invoke(nameof(BackToDefaultPostion), 0.5f);
    }



    private void Update()
    {

        if (grabbable && attempGrab != grabbable.RemoteGrabbing && !grabbable.BeingHeld)
        {
            OnUnGrab();
        }

        //attemp granbed
        attempGrab = grabbable.RemoteGrabbing;
    }


    internal void BackToDefaultPostion()
    {
        if (grabbable == null || grabbable.enabled == false || grabbable.BeingHeld)
            return;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 targetPosition;
        Quaternion targetRotation = defaultRoation;

        if (overrideedTransform != null)
        {
            targetPosition = overrideedTransform.position;
        }
        else
        {
            targetPosition = defaultPosition + Vector3.up * 0.01f;
        }

        // Kill any existing tweens before starting new ones to avoid conflicts
        transform.DOKill();

        // Smoothly move and rotate back to position/rotation
        transform.DOMove(targetPosition, 0.75f) // duration: 0.75 seconds
            .SetEase(Ease.InOutSine);

        transform.DORotateQuaternion(targetRotation, 0.75f)
            .SetEase(Ease.InOutSine);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (grabed)
        {
            return;
        }
        else
        {
            OnUnGrab();
        }
    }

    public void ChangeOverrideTransform(Transform vacutainerOverridePosition, Transform butterflyOverridePosition)
    { 
        chooseNeedleType = FindObjectOfType<ChooseNeedleType>();
        overrideTransform = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (chooseNeedleType.needleType == NeedleType.Vacutainer)
        {
            overrideedTransform =  vacutainerOverridePosition;
        }
        else if (chooseNeedleType.needleType == NeedleType.Butterfly)
        {
            overrideedTransform = butterflyOverridePosition;
        }

    }

    public void ChangeOverrideTransform(Transform target)
    {

        overrideTransform = true;
        overrideedTransform = target;
        ForceDefaultPosition();
        DOVirtual.DelayedCall(0.1f, () =>
        {
          

        });
 

    }

    //--------------------- Added for quiz -----------------------//
    public void ForceDefaultPosition()
    {
        Invoke(nameof(OnForceDefaultPosition), 1.5f);
    }
    public void OnForceDefaultPosition()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 targetPosition;
        Quaternion targetRotation = defaultRoation;

        if (overrideedTransform != null)
        {
            targetPosition = overrideedTransform.position;
        }
        else
        {
            targetPosition = defaultPosition + Vector3.up * 0.01f;
        }

        transform.DOKill();

        transform.DOMove(targetPosition, 0.75f)
            .SetEase(Ease.InOutSine);

        transform.DORotateQuaternion(targetRotation, 0.75f)
            .SetEase(Ease.InOutSine);
    }

    public void ImmediateReturn()
    {
        CancelInvoke(nameof(BackToDefaultPostion));
        grabed = false;
       BackToDefaultPostion();
    }
}

public enum ResetState
{
    Fixed,
    Dynamic
}