using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Scivr.CollisionSequence
{
    public class CollisionSequenceHandler : MonoBehaviour
    {
        [SerializeField] bool isUseSequence;
        [ShowIf("isUseSequence")]
        [SerializeField][ReadOnly] private int currentSequenceIndex = 0;

        [OdinSerialize, ListDrawerSettings(CustomAddFunction = nameof(CreateNewSequenceData))]
        public List<CollisionSequenceData> collisionSequenceList;

        private float accumulatedTimeInTrigger = 0;
        private Vector3 previousPosition;
        private Quaternion previousRotation;
        private float previousTime;
        private bool isInitialStay = true;

        void Start()
        {
            InitializePreviousTransform();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("on trigger enter " + other.name);
            HandleCollision(other, CollisionType.OnTriggerEnter);
        }

        private void OnTriggerStay(Collider other)
        {
            HandleCollision(other, CollisionType.OnTriggerStay);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("on trigger exit " + other.name);
            HandleCollision(other, CollisionType.OnTriggerExit);
        }

        private void HandleCollision(Collider other, CollisionType collisionType)
        {
            if (!IsCurrentIndexValid() || !other.TryGetComponent(out Item item))
            {
                //  Debug.Log($"other {other.name} doesn't have an item");
                return;
            }

            var currentData = collisionSequenceList[currentSequenceIndex];

            if (item.id != currentData.TargetElementId)
                return;

            switch (collisionType)
            {
                case CollisionType.OnTriggerEnter:
                    if (currentData.CollisonType == CollisionType.OnTriggerEnter)
                    {
                        currentData.OnCollsionDone?.Invoke();
                        if (isUseSequence)
                            MoveToNextSequence();
                    }
                    else if (currentData.CollisonType == CollisionType.OnTriggerStayWithMove ||
                             currentData.CollisonType == CollisionType.OnTriggerStayWithRotaion)
                    {
                        InitializePreviousTransform(); // Only initialize previous position and rotation
                    }
                    break;

                case CollisionType.OnTriggerStay:
                    HandleTriggerStay(currentData);
                    break;

                case CollisionType.OnTriggerExit:
                    if (currentData.CollisonType == CollisionType.OnTriggerExit)
                    {
                        currentData.OnCollsionDone?.Invoke();
                        if (isUseSequence)
                            MoveToNextSequence();
                    }
                    else if (!currentData.IsStayWithMoveOrStayWithRotate())
                    {
                        ResetTriggerStay(); // Only reset if not StayWithMove or StayWithRotate
                    }
                    break;
            }
        }

        private void HandleTriggerStay(CollisionSequenceData currentData)
        {
            float deltaTime = Time.time - previousTime;
            previousTime = Time.time;


            if (isInitialStay)
            {
                isInitialStay = false;
                accumulatedTimeInTrigger = 0;
                currentData.OnCollsionStart?.Invoke();
                deltaTime = 0;
            }

            if (currentData.CollisonType == CollisionType.OnTriggerStay)
            {
                accumulatedTimeInTrigger += deltaTime;
                currentData.OnCollisonUpdate?.Invoke();
                Debug.Log("collision in progress " + accumulatedTimeInTrigger);

                if (accumulatedTimeInTrigger >= currentData.TimeSpentInTrigger)
                {
                    currentData.OnCollsionDone?.Invoke();
                    if (isUseSequence)
                        MoveToNextSequence();
                }
            }
            else if (IsSufficientMovementOrRotation(deltaTime, currentData, out float calculatedDeltaTime))
            {
                accumulatedTimeInTrigger += calculatedDeltaTime;
                currentData.OnCollisonUpdate?.Invoke();
                Debug.Log("collision in progress " + accumulatedTimeInTrigger);
                if (accumulatedTimeInTrigger >= currentData.TimeSpentInTrigger)
                {
                    currentData.OnCollsionDone?.Invoke();
                    if (isUseSequence)
                        MoveToNextSequence();
                }
            }
        }

        int currentIteration;
        float samplingTime = 0;

        private bool IsSufficientMovementOrRotation(float deltaTime, CollisionSequenceData currentData, out float calculatedDeltaTime)
        {
            Vector3 currentPosition = transform.position;
            Quaternion currentRotation = transform.rotation;

            float distanceMoved = Vector3.Distance(currentPosition, previousPosition);
            float angleRotated = Quaternion.Angle(currentRotation, previousRotation);

            bool hasMoved = currentData.CollisonType == CollisionType.OnTriggerStayWithMove && distanceMoved >= currentData.MoveMinValue;
            bool hasRotated = currentData.CollisonType == CollisionType.OnTriggerStayWithRotaion && angleRotated >= currentData.RotationMinValue;

            //set sampling time
            samplingTime += deltaTime;

            if (hasMoved || hasRotated)
            {
                // Reset the previous position and rotation since movement/rotation is sufficient
                previousPosition = currentPosition;
                previousRotation = currentRotation;
                currentIteration = 0;

                calculatedDeltaTime = samplingTime;
                samplingTime = 0;

            }
            else
            {
                // Increment the iteration counter
                currentIteration++;
                if (currentIteration > currentData.samplingRate)
                {
                    // Reset the iteration counter and update position/rotation if the sampling rate threshold is reached
                    currentIteration = 0;
                    previousPosition = currentPosition;
                    previousRotation = currentRotation;

                    //rest sampling time
                    samplingTime = 0;

                }
                calculatedDeltaTime = 0;
            }

            //time spent in moving

            return hasMoved || hasRotated;
        }

        private bool IsCurrentIndexValid() => currentSequenceIndex >= 0 && currentSequenceIndex < collisionSequenceList.Count;

        public void MoveToNextSequence()
        {
            currentSequenceIndex++;
            if (currentSequenceIndex < collisionSequenceList.Count)
            {
                var nextData = collisionSequenceList[currentSequenceIndex];
                accumulatedTimeInTrigger = 0; // Only reset if the next type is not StayWithMove or StayWithRotate
                isInitialStay = true;
                InitializePreviousTransform();
            }
        }

        private void ResetTriggerStay()
        {
            accumulatedTimeInTrigger = 0;
            if (isUseSequence)
                MoveToNextSequence();
        }

        private void InitializePreviousTransform()
        {
            previousPosition = transform.position;
            previousRotation = transform.rotation;
            previousTime = Time.time;
        }

        private CollisionSequenceData CreateNewSequenceData() => new CollisionSequenceData();

        private void OnDrawGizmos()
        {
            foreach (var sequenceData in collisionSequenceList)
            {
                if (sequenceData.ShowGizmos)
                {
                    if (sequenceData.IsMove())
                    {
                        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * sequenceData.MoveMinValue);
                    }
                    else if (sequenceData.IsRotation())
                    {
                        // Create a quaternion representing the rotation around the up axis (Vector3.up)
                        Quaternion rotation = Quaternion.AngleAxis(sequenceData.RotationMinValue, Vector3.up);
                        // Apply the rotation to the vector
                        var rotatedVector = rotation * (transform.position + Vector3.right);
                        Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
                        Gizmos.DrawLine(transform.position, rotatedVector);
                    }
                }
            }
        }
    }
}



