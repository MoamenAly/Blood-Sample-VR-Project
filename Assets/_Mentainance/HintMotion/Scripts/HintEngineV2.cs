using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Scivr.HintEngine
{
    public class HintEngineV2 : MonoSinglton<HintEngineV2>
    {
        private HintMotion hintMotion = null;
        public HandPoser handPoser;
        public HintMotion[] hintMotions;

        private void Start()
        {
            for (int i = 0; i < hintMotions.Length; i++)
            {
                if (hintMotions[i].sourceMesh!=null)
                hintMotions[i].roation = hintMotions[i].sourceMesh.transform.rotation;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Update the item names with their indices
            for (int i = 0; i < hintMotions.Length; i++)
            {
                hintMotions[i].hintMotionsName = $"Motion {i}";
            }
        }
#endif

        public void SetActiveMotion(int index) {
            if (index == 0 && (PlatformManager.Instance.platform == Platform.Webgl)) return;
            this.hintMotion = hintMotions[index];
        }

        public void ActiveHint() {
            if (hintMotion != null)
            {
                hintMotion.ActivateMotion(handPoser);
            }
        }

        [Button]
        public void ActiveMotion(int index)
        {
            if (index == 0 && (PlatformManager.Instance.platform == Platform.Webgl)) return;
            this.hintMotion = hintMotions[index];
            if (hintMotion != null)
            {
                hintMotion.ActivateMotion(handPoser);
            }
        }


      

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(hintMotion != null)
            hintMotion.OnDrawGizmos();
        }


#endif

        internal void RestCurrentHint()
        {
            if (hintMotion != null)
            {
                hintMotion?.Stop();
            }
            hintMotion = null;
        }

        internal HintMotion GetActiveMotion()
        {
            return  this.hintMotion;
        }
    }

    [System.Serializable]
    public class HintMotion
    {
        //[FoldoutGroup("$hintMotionsName")]
        [TextArea(3, 10)] public string description;
        //[FoldoutGroup("$hintMotionsName")]
        //public Transform source;
        //[FoldoutGroup("$hintMotionsName")]
        public GameObject sourceMesh;

        public Transform target;
        // [FoldoutGroup("$hintMotionsName")]
        // [FoldoutGroup("$hintMotionsName")]
        internal bool IsMotionPlaying = false;
        // [FoldoutGroup("$hintMotionsName")]
        [SerializeField] float snapTime = 3f;
        // [FoldoutGroup("$hintMotionsName")]
        private HandPoser handPoser;
        //  [FoldoutGroup("$hintMotionsName")]
        [SerializeField]public Vector3 controllPointOffest;
        //  [FoldoutGroup("$hintMotionsName")]
        [SerializeField]public float hideDelay = 1;
        //[SerializeField]public Vector3 controllPointOffest2; 
        //   [FoldoutGroup("$hintMotionsName")]
        [Space] public UnityEvent2 OnMotionStarted;
        //   [FoldoutGroup("$hintMotionsName")]
        [Space] public UnityEvent2 OnMotionCompleted;
        internal string hintMotionsName;

        Transform parent;
        Vector3 startPosition;
        //  public bool Rest = false;
        // HandPoser poser;
        internal Quaternion roation;

        public void ActivateMotion(HandPoser poser)
        {
            if (IsMotionPlaying || !Application.isPlaying)
            {
                Debug.Log("need to edited in run time");
                return;
            }
            else
            {
                StartMotion(poser);
                handPoser = poser;
            }
        }

        //Vector3 ConvertLocalToGlobalEulerAngles(Transform targetTransform, Vector3 localEulerAngles)
        //{
        //    // Create a rotation from the local Euler angles
        //    Quaternion localRotation = Quaternion.Euler(localEulerAngles);

        //    // Convert the local rotation to global rotation
        //    Quaternion globalRotation = targetTransform.parent.rotation * localRotation;

        //    // Return the global Euler angles
        //    return globalRotation.eulerAngles;
        //}

        private void StartMotion(HandPoser poser)
        {

            if (sourceMesh == null || target==null) {
                OnMotionStarted?.Invoke();
            }

            handPoser = poser;

            Vector3[] points = GetPath();
            
            parent = sourceMesh.transform.parent;
            sourceMesh.transform.rotation = roation;
          
            if (points == null) return;

            startPosition = sourceMesh.transform.localPosition;
            // Quaternion rot = sourceMesh.transform.rotation;

            var grabPoints = sourceMesh.GetComponentsInChildren<GrabPoint>(true);


            for (int i = 0; i < grabPoints.Length; i++)
            {
                if (grabPoints[i].RightHandIsValid)
                {
                    sourceMesh.transform.position = points[0];
                    poser.CurrentPose = grabPoints[i].SelectedHandPose;
                    poser.DoPoseUpdate();
                    poser.transform.parent  = sourceMesh.transform;                   
                  
                    poser.transform.position = grabPoints[i].transform.position;
                    poser.transform.rotation = grabPoints[i].transform.rotation;

                    sourceMesh.transform.parent = null; 
                    poser.gameObject.SetActive(true);
                    sourceMesh.gameObject.SetActive(true);
                }
            }

                IsMotionPlaying = true;

             

                CustomBezierPath.DOBezierPath(sourceMesh.transform, points, snapTime,
                OnMotionStarted: () =>
                {
                    OnMotionStarted?.Invoke();
                },
                OnMotionCompleted: () =>
                {
                    //if (Rest)
                  //  {
                        DOVirtual.DelayedCall(hideDelay, () =>
                        {
                            IsMotionPlaying = false;
                            poser.gameObject.SetActive(false);
                            sourceMesh.gameObject.SetActive(false);
                            poser.transform.parent = null;
                            sourceMesh.transform.parent = parent;
                            sourceMesh.transform.localPosition = startPosition;
                            sourceMesh.transform.rotation = roation;

                        });
                  //  }
                  
                    OnMotionCompleted?.Invoke();                   
                }
            );

        }

        internal Vector3[] GetPath()
        {

            if (target==null || sourceMesh==null) {
                return null;
            }

            Vector3 targetPosition = target.position;
            Vector3 objectPosition = sourceMesh.transform.position;
            Vector3 centerOffset   = this.controllPointOffest;

            Vector3[] points = new Vector3[3];

            points[0] = objectPosition;

            Vector3 intermediatePoint = new Vector3((objectPosition.x + targetPosition.x) / 2, (objectPosition.y + targetPosition.y) / 2, (objectPosition.z + targetPosition.z) / 2) + centerOffset;
            points[1] = intermediatePoint;

          
            points[2] = targetPosition;

            return points;
        }   


#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Vector3[] pathPoints = GetPath();

            if (pathPoints != null && pathPoints.Length == 3)
            {
                BezierDrawer.DrawBezierPath(pathPoints[0], pathPoints[1], pathPoints[2], 20);
            }
        }


#endif

        internal void Stop()
        {
            if (IsMotionPlaying)
            {
                if (handPoser != null)
                {
                    handPoser?.gameObject.SetActive(false);
                    handPoser.gameObject.SetActive(false);
                    handPoser.transform.parent = null;
                }

                if (sourceMesh != null)
                {
                    sourceMesh?.gameObject.SetActive(false);


                    sourceMesh.gameObject.SetActive(false);
                    sourceMesh.transform.parent = parent;
                    sourceMesh.transform.localPosition = startPosition;
                    sourceMesh.transform.rotation = roation;
                }
                IsMotionPlaying = false;
            }
        }
    }
}
    public class BezierDrawer
    {
        public static void DrawBezierPath(Vector3 start, Vector3 control, Vector3 end, int bezierSegments)
        {
            Vector3 previousPoint = start;

            for (int i = 1; i <= bezierSegments; i++)
            {
                float t = i / (float)bezierSegments;
                Vector3 currentPoint = CalculateBezierPoint(t, start, control, end);
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }

        public static Vector3 CalculateBezierPoint(float t, Vector3 start, Vector3 control, Vector3 end)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * start; // u^2 * P0
            point += 2 * u * t * control; // 2 * u * t * P1
            point += tt * end; // t^2 * P2

            return point;
        }

    }

