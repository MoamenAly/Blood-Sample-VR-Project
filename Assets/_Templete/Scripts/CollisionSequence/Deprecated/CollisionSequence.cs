using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSequence : MonoBehaviour
{

    private int index = 0;
    [OdinSerialize, ListDrawerSettings(CustomAddFunction = nameof(CreateNewItem))]
    public List<ColliosionSequenceData> colliosionSequenceDatas;
    float timeSpentInTrigger = 0;
    float enterTime = 0;


    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private float lastTime;

    private void OnTriggerEnter(Collider other)
    {
        if (index >= 0 && index < colliosionSequenceDatas.Count) {
           
            other.TryGetComponent(out Item item);
            bool collideWithTargt = item.id == colliosionSequenceDatas[index]._targetElementId;
            if (item && collideWithTargt)
            {
                bool isTargetMethod = CollisonType.OnTriggerEnter == colliosionSequenceDatas[index].collisonType;
                if (isTargetMethod)
                {
                  colliosionSequenceDatas[index].unityEvent?.Invoke();
                  index++;
                }
                enterTime = Time.time;
                // Initialize last position and rotation
                lastPosition = other.transform.position;
                lastRotation = other.transform.rotation;
                lastTime = Time.time;
            }
            
        }
    }

    private ColliosionSequenceData CreateNewItem()
    {
        return new ColliosionSequenceData();
    }

    private void OnTriggerStay(Collider other)
    {
        if (index >= 0 && index < colliosionSequenceDatas.Count)
        {
            other.TryGetComponent(out Item item);
            bool collideWithTargt = item.id == colliosionSequenceDatas[index]._targetElementId;
            if (item && collideWithTargt)
            {
                bool OnTriggerStay = CollisonType.OnTriggerStay == colliosionSequenceDatas[index].collisonType;
                bool OnTriggerStayWithMove = CollisonType.OnTriggerStayWithMove == colliosionSequenceDatas[index].collisonType;
                bool OnTriggerStayWithRoation = CollisonType.OnTriggerStayWithRotaion == colliosionSequenceDatas[index].collisonType;
                if (OnTriggerStay)
                {
                    timeSpentInTrigger = Time.time - enterTime;
                    if (timeSpentInTrigger >= colliosionSequenceDatas[index].timeSpentInTrigger)
                    {
                        colliosionSequenceDatas[index].unityEvent?.Invoke();
                        index++;
                    }
                }else if(OnTriggerStayWithMove|| OnTriggerStayWithRoation)
                {
                    Debug.Log(1);
                    // Calculate motion amount
                    float deltaTime = Time.time - lastTime;
                    deltaTime = deltaTime > 0 ? deltaTime : 1;
                    Debug.Log("delata time " + deltaTime);
                    Vector3 currentPosition = other.transform.position;
                    Quaternion currentRotation = other.transform.rotation;

                    float distanceMoved = Vector3.Distance(currentPosition, lastPosition) / deltaTime;
                    float angleRotated = Quaternion.Angle(currentRotation, lastRotation) / deltaTime;

                    
                    Debug.Log(2);

                    // Check movement and rotation separately
                    bool hasMoved = distanceMoved >= colliosionSequenceDatas[index].moveMinvalue;
                    bool hasRotated = angleRotated >= colliosionSequenceDatas[index].rotationMinvalue;
                    Debug.Log("3 "+distanceMoved +" - "+ colliosionSequenceDatas[index].moveMinvalue);
                    Debug.Log("4 "+ angleRotated +" - "+ colliosionSequenceDatas[index].rotationMinvalue);

                    if (colliosionSequenceDatas[index].collisonType == CollisonType.OnTriggerStayWithMove && hasMoved ||
                        colliosionSequenceDatas[index].collisonType == CollisonType.OnTriggerStayWithRotaion && hasRotated)
                    {


                        Debug.Log("4.1 " + Time.time +" - "+ lastTime);
                        timeSpentInTrigger += Time.time - lastTime;

                        if (timeSpentInTrigger >= colliosionSequenceDatas[index].timeSpentInTrigger)
                        {
                            Debug.Log("5");
                            colliosionSequenceDatas[index].unityEvent?.Invoke();
                            index++;
                        }
                    }
                    Debug.Log("5.1 " + timeSpentInTrigger);

                    lastPosition = currentPosition;
                    lastRotation = currentRotation;
                    lastTime = Time.time;
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (index >= 0 && index < colliosionSequenceDatas.Count)
        {
            other.TryGetComponent(out Item item);

            bool collideWithTargt = item.id == colliosionSequenceDatas[index]._targetElementId;

            if (item && collideWithTargt)
            {
                bool isTargetMethod = CollisonType.OnTriggerExit == colliosionSequenceDatas[index].collisonType;
                if (isTargetMethod)
                {
                    colliosionSequenceDatas[index].unityEvent?.Invoke();
                    index++;
                }
                enterTime = Time.time;
                timeSpentInTrigger = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < colliosionSequenceDatas.Count; ++i)
        {
            if (colliosionSequenceDatas[i].IsStayWithMoveOrStayWithRotate()) {
                Gizmos.DrawLine(transform.position, transform.position + Vector3.right * colliosionSequenceDatas[i].moveMinvalue);
            }
        }
    }

}


public enum CollisonType { 
 OnTriggerEnter = 1<<0, OnTriggerExit=1<<1, OnTriggerStay=1<<2,OnTriggerStayWithMove=1<<3, OnTriggerStayWithRotaion = 1 << 4
}


