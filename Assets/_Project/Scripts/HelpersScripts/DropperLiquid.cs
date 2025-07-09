using BNG;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum DropperMode
{
    Manual = 0,
    Automatic = 1,
}

public class DropperLiquid : MonoBehaviour
{
    public DropperMode mode;

    public GameObject dropPrefab;
    public GameObject attachTransform;
    //public GameObject dropperLiquidObject;

    public AudioSource sfxSource;
    public AudioClip dropEffect;

    //public SimpleDependency _simpleDependency;

    public float manualDelay = 0.7f;
    public float automaticDelay = 0.7f;
    public float destroyDelay = 0.2f;

    GrabbableUnityEvents _grabbableUnityEvents;

    bool isGrabbed = false;

    private bool _isInstantiated;

    [SerializeField] bool _canDrop;
    public bool canDrop { get { return _canDrop; } set { _canDrop = value; } }

    private void Awake()
    {
        //if (!gameObject.TryGetComponent<GrabbableUnityEvents>(out _grabbableUnityEvents)) _grabbableUnityEvents = gameObject.AddComponent<GrabbableUnityEvents>();
    }

    void Start()
    {
        if (_grabbableUnityEvents == null) return;

        _grabbableUnityEvents.onGrab.AddListener(SetGrabbed);
        _grabbableUnityEvents.onRelease.AddListener(SetReleased);
    }

    void SetGrabbed(Grabber grabber)
    {
        isGrabbed = true;
    }

    void SetReleased()
    {
        isGrabbed = false;
    }

    private void Update()
    {
        if (mode == DropperMode.Manual)
        {
            if (InputBridge.Instance.RightTriggerDown && !_isInstantiated && isGrabbed)
            {
                //if (_simpleDependency.IsCompleted() && dropperLiquidObject.activeSelf) StartCoroutine(InstantiateLiquid_Coroutine(manualDelay));
                if (_canDrop) StartCoroutine(InstantiateLiquid_Coroutine(manualDelay));
            }
        }
    }

    [Button]
    public void InstantiateLiquidAutomatic()
    {
        StartCoroutine(InstantiateLiquidAutomatic_Coroutine());
    }

    public void InstantiateLiquidAutomatic(int dropsCount)
    {
        StartCoroutine(InstantiateLiquidAutomatic_Coroutine(dropsCount));
    }

    public void StopInstantiateLiquidAutomatic()
    {
        StopCoroutine(InstantiateLiquidAutomatic_Coroutine());
        _isInstantiated = false;
    }
    IEnumerator InstantiateLiquidAutomatic_Coroutine()
    {
        while (_canDrop)
        {
            yield return StartCoroutine(InstantiateLiquid_Coroutine(automaticDelay));
        }
    }

    IEnumerator InstantiateLiquidAutomatic_Coroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return StartCoroutine(InstantiateLiquid_Coroutine(automaticDelay));
        }
    }

    IEnumerator InstantiateLiquid_Coroutine(float delay)
    {
        _isInstantiated = true;
        sfxSource.PlayOneShot(dropEffect);

        GameObject instantiatedPrefab = Instantiate(dropPrefab, attachTransform.transform.position, attachTransform.transform.rotation, attachTransform.transform);
        instantiatedPrefab.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        instantiatedPrefab.transform.forward = Vector3.up;

        yield return new WaitForSeconds(destroyDelay);
        Destroy(instantiatedPrefab.gameObject);

        yield return new WaitForSeconds(delay);
        _isInstantiated = false;
    }

    public void IncreaseSliderProgress(Slider2 dropperSlider)
    {
        dropperSlider.value += 0.1f;
    }
}
