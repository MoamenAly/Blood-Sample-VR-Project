using LiquidVolumeFX;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class LiquidContainer : MonoBehaviour
{

    private CustomGrabbable customGrabbable;

    [SerializeField] private LayerMask interactionLayer = (1 << 6) | (1 << 7);
    [SerializeField] private LayerMask onHadnLayer = 1 << 7; //not pouring
    [SerializeField] private LayerMask pouringLayer = 1 << 8;

    private GameObject currentContactedObject;
    private LiquidContainer otherLiquidContainer;

    [SerializeField] private bool isSourceContainer = false;

    //pourin source setting 
    [ShowIf(nameof(isSourceContainer))]
    public List<LiquidPouringSequence> pouringSequences;
    [ShowIf(nameof(isSourceContainer))]
    [SerializeField] private Vector3 detectionBoxSize;
    [ShowIf(nameof(isSourceContainer))]
    [SerializeField] private float detectionBoxLength;

    private int currentSequenceIndex = 0;
    [SerializeField] private LiquidVolume liquidVisual;

    [Header("Liquad")]
    public float currentVolumeInMilliliters = 0;
    public float maxVolumeInMilliGram = 0;

    private bool isPouring = false;

    public bool isPouringEnabled = true;
    Vector3 spillPoint;
    bool carriedOnHand;
    int defaultLayer;

    [SerializeField] EffectHolder effectHolder;
    public bool isUseSpillPoint = true;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (liquidVisual && liquidVisual.liquidLayers.Length > 0 && maxVolumeInMilliGram > 0)
        {
            liquidVisual.level = (float)(currentVolumeInMilliliters / maxVolumeInMilliGram);
            liquidVisual.liquidLayers[0].amount = (float)(currentVolumeInMilliliters / maxVolumeInMilliGram);
            liquidVisual.UpdateLayers(true);
        }
    }
#endif

    private void Start()
    {
        defaultLayer = gameObject.layer;

        customGrabbable = GetComponent<CustomGrabbable>();
        otherLiquidContainer = null;
        currentContactedObject = null;
        if (liquidVisual && liquidVisual.liquidLayers.Length > 0 && maxVolumeInMilliGram > 0)
        {
            liquidVisual.level = currentVolumeInMilliliters / maxVolumeInMilliGram;
            liquidVisual.liquidLayers[0].amount = currentVolumeInMilliliters / maxVolumeInMilliGram;
            liquidVisual.UpdateLayers(true);
        }
    }

    private void Update()
    {
        if (!isPouringEnabled) return;

        bool isReadyToPour = IsReadyToPour();

        UpdateThisLayerToAvoidItSelf(isReadyToPour);

        if (isReadyToPour)
        {

            UpdatePouringTarget();

            if (otherLiquidContainer != null)
            {
                StartPouring(otherLiquidContainer);
            }
            else
            {
                StopPouring();
            }
        }
        else
        {
            StopPouring();
        }

        if (isPouring)
        {
            OnPouring();
        }
    }

    void OnPouring()
    {
        var currentSequence = pouringSequences[currentSequenceIndex];
        if (currentSequence != null && otherLiquidContainer.isPouringEnabled)
        {
            //still filling
            if (currentSequence.fillAmountInMilligrams > 0.0f)
            {
                if (currentVolumeInMilliliters > 0.0f)
                {
                    //calculate fill amount
                    float fillAmount = currentSequence.fillSpeed * Time.fixedDeltaTime;
                    fillAmount = Mathf.Round(fillAmount * 10f) / 10f; // Rounds to one decimal place (3.1)

                    fillAmount = fillAmount > currentSequence.fillAmountInMilligrams ? currentSequence.fillAmountInMilligrams : fillAmount;

                    currentSequence.fillAmountInMilligrams -= fillAmount;

                    //update me 
                    UpdateLiquidVolume(-fillAmount);

                    //update othor
                    otherLiquidContainer.UpdateLiquidVolume(fillAmount);

                    if (currentSequence.fillAmountInMilligrams == 0)
                    {
                        //fill done
                        currentSequence.OnFillDone?.Invoke();
                        currentSequenceIndex++;
                        otherLiquidContainer = null;
                        currentContactedObject = null;
                    }

                }
            }
            else
            {
                //fill done
                //currentSequence.OnFillDone?.Invoke();
                //currentSequenceIndex++;
                //othorLiquadContainter = null;
                //currentContactObject = null;
            }
        }
    }

    internal void UpdateLiquidVolume(float fillAmount)
    {
        currentVolumeInMilliliters = currentVolumeInMilliliters + fillAmount;
        liquidVisual.level = (float)(currentVolumeInMilliliters / maxVolumeInMilliGram);
        liquidVisual.liquidLayers[0].amount = (float)(currentVolumeInMilliliters / maxVolumeInMilliGram);
        liquidVisual.UpdateLayers(true);
    }

    private void StartPouring(LiquidContainer othorLiquad)
    {
        if (isUseSpillPoint == true)
            effectHolder.transform.position = spillPoint;

        effectHolder.Play();
        isPouring = true;
    }

    private void StopPouring()
    {
        effectHolder.Stop();
        isPouring = false;
    }

    private void UpdateThisLayerToAvoidItSelf(bool readyToPour)
    {
        LayerMask layerMask = readyToPour ? pouringLayer : onHadnLayer;
        if (!carriedOnHand)
        {
            gameObject.layer = defaultLayer;
        }
        else
        {
            gameObject.layer = ConvertLayerMaskToLayer(layerMask);
        }
    }

    private bool IsReadyToPour()
    {
        bool haveGoodRotation = liquidVisual.GetSpillPoint(out spillPoint);
        carriedOnHand = customGrabbable != null && customGrabbable.BeingHeld;

        if (PlatformManager.Instance.platform == Platform.Webgl /*|| Application.platform == RuntimePlatform.WindowsEditor*/)
            carriedOnHand = true;

        bool onSequenceBounds = currentSequenceIndex < pouringSequences.Count;
        return haveGoodRotation && carriedOnHand && onSequenceBounds;
    }

    private void UpdatePouringTarget()
    {
        RaycastHit hitInfo;
        bool isHitSomething = Physics.BoxCast(spillPoint + Vector3.up * 0.1f, detectionBoxSize / 2, Vector3.down, out hitInfo, Quaternion.identity, detectionBoxLength, interactionLayer);
        if (isHitSomething)
        {
            if (IsCurrentTargetUpdated(hitInfo))
            {
                otherLiquidContainer = GetTargetContainer(hitInfo);
            }
            currentContactedObject = hitInfo.collider.gameObject;
            Debug.Log("hit " + hitInfo.collider.gameObject.name);
        }
        else
        {
            Debug.Log("no hit detected");
            otherLiquidContainer = null;
            currentContactedObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        bool haveGoodRotation = liquidVisual.GetSpillPoint(out spillPoint);
        if (haveGoodRotation)
        {
            Gizmos.DrawWireSphere(spillPoint + Vector3.up * 0.1f, 0.005f);

            Gizmos.color = Color.red;
            Vector3 start = spillPoint + Vector3.up * 0.1f;
            Vector3 end = start + Vector3.down * detectionBoxLength;

            // Draw the start box
            Gizmos.DrawWireCube(start, detectionBoxSize);

            // Draw the end box
            Gizmos.DrawWireCube(end, detectionBoxSize);

            // Draw lines connecting the start and end boxes
            Gizmos.DrawLine(start + new Vector3(detectionBoxSize.x / 2, detectionBoxSize.y / 2, detectionBoxSize.z / 2), end + new Vector3(detectionBoxSize.x / 2, detectionBoxSize.y / 2, detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(-detectionBoxSize.x / 2, detectionBoxSize.y / 2, detectionBoxSize.z / 2), end + new Vector3(-detectionBoxSize.x / 2, detectionBoxSize.y / 2, detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(detectionBoxSize.x / 2, -detectionBoxSize.y / 2, detectionBoxSize.z / 2), end + new Vector3(detectionBoxSize.x / 2, -detectionBoxSize.y / 2, detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(detectionBoxSize.x / 2, detectionBoxSize.y / 2, -detectionBoxSize.z / 2), end + new Vector3(detectionBoxSize.x / 2, detectionBoxSize.y / 2, -detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(-detectionBoxSize.x / 2, -detectionBoxSize.y / 2, detectionBoxSize.z / 2), end + new Vector3(-detectionBoxSize.x / 2, -detectionBoxSize.y / 2, detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(detectionBoxSize.x / 2, -detectionBoxSize.y / 2, -detectionBoxSize.z / 2), end + new Vector3(detectionBoxSize.x / 2, -detectionBoxSize.y / 2, -detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(-detectionBoxSize.x / 2, detectionBoxSize.y / 2, -detectionBoxSize.z / 2), end + new Vector3(-detectionBoxSize.x / 2, detectionBoxSize.y / 2, -detectionBoxSize.z / 2));
            Gizmos.DrawLine(start + new Vector3(-detectionBoxSize.x / 2, -detectionBoxSize.y / 2, -detectionBoxSize.z / 2), end + new Vector3(-detectionBoxSize.x / 2, -detectionBoxSize.y / 2, -detectionBoxSize.z / 2));
        }
    }

    private bool IsCurrentTargetUpdated(RaycastHit hitInfo)
    {
        return hitInfo.collider.gameObject != currentContactedObject;
    }

    private LiquidContainer GetTargetContainer(RaycastHit hitInfo)
    {
        hitInfo.collider.gameObject.TryGetComponent(out Item item);
        if (item && item.id == pouringSequences[currentSequenceIndex].targetId)
        {
            if (item.TryGetComponent(out LiquidContainer otherContainer))
            {
                return otherContainer;
            }
        }
        return null;
    }

    private int ConvertLayerMaskToLayer(LayerMask layerMask)
    {
        int layer = 0;
        int layerMaskValue = layerMask.value;
        while (layerMaskValue > 0)
        {
            layerMaskValue >>= 1;
            layer++;
        }
        return layer - 1;
    }

    public void _EnblePouring()
    {
        isPouringEnabled = true;
    }

    public void _DisablePouring()
    {
        isPouringEnabled = false;
    }


    Color currrent, target;
    float spentTime = 0;
    float lerpTime;

    public void OnStatColorChange(Color target, float lerpTime)
    {
        currrent = liquidVisual.liquidLayers[0].color;
        this.target = target;
        spentTime = 0;
        this.lerpTime = lerpTime;
    }

    public void LerpToColor()
    {
        float value = spentTime / lerpTime;
        Debug.Log(value);
        spentTime += Time.deltaTime;
        liquidVisual.liquidLayers[0].color = Color.Lerp(currrent, target, value);
        liquidVisual.UpdateLayers(true);

    }


}

[System.Serializable]
public class LiquidPouringSequence
{
    public Elements targetId;
    public float fillSpeed;
    public float fillAmountInMilligrams;
    public UnityEvent2 OnFillDone;
}


//public class LiquadData {

//    [SerializeField] public Color color;
//    [SerializeField] public bool mix = false;
//    [SerializeField][DropdownList("liquad_ids")] public string id;
//}
