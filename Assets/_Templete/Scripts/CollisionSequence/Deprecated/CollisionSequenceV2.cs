using Scivr.HintEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSequenceV2 : MonoBehaviour
{
    private int index = 0;
    [OdinSerialize, ListDrawerSettings(CustomAddFunction = nameof(CreateNewItem))]
    public List<ColliosionSequenceData> colliosionSequenceDatas;

    float timeSpentInTrigger = 0;
    //float enterTime = 0;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private float lastTime;
    private bool isFirstStay = true;

    private void Start()
    {
        
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Update the item names with their indices
        for (int i = 0; i < colliosionSequenceDatas.Count; i++)
        {
            colliosionSequenceDatas[i].CollisionName = $"Collision {i}";
        }
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if (index >= 0 && index < colliosionSequenceDatas.Count)
        {
            if (other.TryGetComponent(out Item item))
            {
                bool collideWithTarget = item.id == colliosionSequenceDatas[index]._targetElementId;
                if (item && collideWithTarget)
                {
                    if (colliosionSequenceDatas[index].collisonType == CollisonType.OnTriggerEnter)
                    {
                        colliosionSequenceDatas[index].unityEvent?.Invoke();
                        index++;
                    }
                   // enterTime = Time.time;

                    // Initialize last position and rotation
                    lastPosition = transform.position;
                    lastRotation = transform.rotation;
                    lastTime = Time.time;
                    isFirstStay = true;
                }
            }
        }
    }


    private ColliosionSequenceData CreateNewItem()
    {
        return new ColliosionSequenceData();
    }

    private int cashedIndex = -1; 


    private void OnTriggerStay(Collider other)
    {
        if (index >= 0 && index < colliosionSequenceDatas.Count)
        {
            if (other.TryGetComponent(out Item item))
            {
                bool collideWithTarget = item.id == colliosionSequenceDatas[index]._targetElementId;
                if (item && collideWithTarget)
                {   
                    CollisonType collisonType = colliosionSequenceDatas[index].collisonType;

                    if (collisonType == CollisonType.OnTriggerStay)
                    {                       
                        float deltaTime;
                        if (isFirstStay)
                        {
                            deltaTime = 0;
                            timeSpentInTrigger = 0;
                            isFirstStay = false;
                            colliosionSequenceDatas[index].OnStart?.Invoke();


                        }
                        else
                        {
                            deltaTime = Time.time - lastTime;
                        }
                       
                        //update last time
                        lastTime = Time.time;

                        timeSpentInTrigger += deltaTime;

                        colliosionSequenceDatas[index].OnUpdate?.Invoke();

                        if (timeSpentInTrigger >= colliosionSequenceDatas[index].timeSpentInTrigger)
                        {
                            colliosionSequenceDatas[index].unityEvent?.Invoke();
                            timeSpentInTrigger = 0;
                            index++;
                        }
                    }
                    else if (collisonType == CollisonType.OnTriggerStayWithMove || collisonType == CollisonType.OnTriggerStayWithRotaion)
                    {
                        // Calculate motion amount
                        float deltaTime;
                        if (isFirstStay)
                        {
                            deltaTime = 0;
                            isFirstStay = false;
                            colliosionSequenceDatas[index].OnStart?.Invoke();

                        }
                        else
                        {
                            deltaTime = Time.time - lastTime;                          
                        }

                        Vector3 currentPosition = transform.position;
                        Quaternion currentRotation = transform.rotation;

                        float distanceMoved = deltaTime > 0 ? Vector3.Distance(currentPosition, lastPosition) / deltaTime : 0;
                        float angleRotated = deltaTime > 0 ? Quaternion.Angle(currentRotation, lastRotation) / deltaTime : 0;

                        lastPosition = currentPosition;
                        lastRotation = currentRotation;
                        lastTime = Time.time;

                        // Check movement and rotation separately
                        bool hasMoved = distanceMoved >= colliosionSequenceDatas[index].moveMinvalue;
                        bool hasRotated = angleRotated >= colliosionSequenceDatas[index].rotationMinvalue;

                        if ((collisonType == CollisonType.OnTriggerStayWithMove && hasMoved) ||
                            (collisonType == CollisonType.OnTriggerStayWithRotaion && hasRotated))
                        {
                            timeSpentInTrigger += deltaTime;
                            colliosionSequenceDatas[index].OnUpdate?.Invoke();
                            if (timeSpentInTrigger >= colliosionSequenceDatas[index].timeSpentInTrigger)
                            {
                                colliosionSequenceDatas[index].unityEvent?.Invoke();
                                index++;
                            }
                        }

                    }

                   // colliosionSequenceDatas[index].unityEvent?.Invoke();
                    Debug.Log("timeSpentInTrigger " + timeSpentInTrigger);

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (index >= 0 && index < colliosionSequenceDatas.Count)
        {
            if (other.TryGetComponent(out Item item))
            {
                bool collideWithTarget = item.id == colliosionSequenceDatas[index]._targetElementId;
                if (item && collideWithTarget)
                {
                    if (colliosionSequenceDatas[index].collisonType == CollisonType.OnTriggerExit)
                    {
                        colliosionSequenceDatas[index].unityEvent?.Invoke();
                        index++;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < colliosionSequenceDatas.Count; ++i)
        {
            if (colliosionSequenceDatas[i].IsStayWithMoveOrStayWithRotate())
            {
                Gizmos.DrawLine(transform.position, transform.position + Vector3.right * colliosionSequenceDatas[i].moveMinvalue);
            }
        }

        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}