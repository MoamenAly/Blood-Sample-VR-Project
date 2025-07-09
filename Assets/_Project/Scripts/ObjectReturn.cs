using BNG;
using System.Collections;
using UnityEngine;

public class ObjectReturn : MonoBehaviour
{
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Vector3 startingScale;

    [SerializeField] private bool _isSnapped;
    public bool isSnapped
    {
        get { return _isSnapped; } set { _isSnapped = value; }
    }

    [SerializeField] private bool _isGrabbed;
    public bool isGrabbed
    {
        get { return _isGrabbed; } set { _isGrabbed = value; }
    }

    [SerializeField] private Transform parentTransform;

    GrabbableUnityEvents _grabbableUnityEvents;

    void Awake()
    {
        if(PlatformManager.Instance.platform == Platform.Webgl)
        {
            isGrabbed = true;
        }
        else
        {
            isGrabbed = false;
        }
        if (!gameObject.TryGetComponent<GrabbableUnityEvents>(out _grabbableUnityEvents)) _grabbableUnityEvents = gameObject.AddComponent<GrabbableUnityEvents>();

        InvokeRepeating("CheckPosition", 0, 1f);
    }

    private void CheckPosition()
    {
        if (!_isGrabbed && !_isSnapped)
        {
            if (Vector3.Distance(transform.position, startingPosition) > 0.01)
            {
                ReturnToStartTransform();
            }
        }
    }

    private void OnEnable()
    {
        _grabbableUnityEvents.onGrab.AddListener(SetGrabbed);

        _grabbableUnityEvents.onRelease.AddListener(ReturnToStartTransform);
        _grabbableUnityEvents.onRelease.AddListener(SetReleased);

        _grabbableUnityEvents.onSnapZoneEnter.AddListener(SnapZoneEnter);

        _grabbableUnityEvents.onSnapZoneExit.AddListener(SnapZoneExit);
        _grabbableUnityEvents.onSnapZoneExit.AddListener(ReturnToStartTransform);

        SaveStartingTransform();
    }

    private void OnDisable()
    {
        _grabbableUnityEvents.onGrab.RemoveListener(SetGrabbed);

        _grabbableUnityEvents.onRelease.RemoveListener(ReturnToStartTransform);
        _grabbableUnityEvents.onRelease.RemoveListener(SetReleased);

        _grabbableUnityEvents.onSnapZoneEnter.RemoveListener(SnapZoneEnter);

        _grabbableUnityEvents.onSnapZoneExit.RemoveListener(SnapZoneExit);
        _grabbableUnityEvents.onSnapZoneExit.RemoveListener(ReturnToStartTransform);
    }

    void SaveStartingTransform()
    {
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;
        startingScale = transform.localScale;

        parentTransform = transform.parent;
    }

    void SnapZoneEnter()
    {
        isSnapped = true;
    }

    void SnapZoneExit()
    {
        isSnapped = false;
    }

    void SetGrabbed(Grabber grabber)
    {
        isGrabbed = true;
    }

    void SetReleased()
    {
        isGrabbed = false;
    }

    void ReturnToStartTransform()
    {
        
        if(gameObject.activeInHierarchy)
            StartCoroutine(ReturnToStartTransform_Coroutine());
    }

    IEnumerator ReturnToStartTransform_Coroutine()
    {
        yield return new WaitForSeconds(1.5f);
        if (!_isSnapped && !_isGrabbed)
        {
            transform.parent = parentTransform;

            transform.localScale = startingScale;
            transform.localRotation = startingRotation;
            transform.localPosition = startingPosition;
        }
    }

    public void InstantReturnToStartTransform()
    {
        if (!_isSnapped && !_isGrabbed)
        {
            transform.parent = parentTransform;

            transform.localScale = startingScale;
            transform.localRotation = startingRotation;
            transform.localPosition = startingPosition;
        }
    }
}