using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace BNG
{
    public class RemoteGrabHint : MonoBehaviour
    {
        string itemName;

        public float tweenDuration = 0.15f;
        private const float upOffest = 0.2f;

        Canvas canvas;
        RTLTMPro.RTLTextMeshPro text;

        bool isShowing = false;

        Transform mainCam;
        Vector3 initialScale;


        Grabbable currentGrabbable;
        [SerializeField] GrabbablesInTrigger leftGrabbablesInTrigger;
        [SerializeField] GrabbablesInTrigger rightGrabbablesInTrigger;

        [SerializeField] Grabber leftGrabber;
        [SerializeField] Grabber righttGrabber;

        void Start()
        {
            AssignCamera();

            canvas = GetComponent<Canvas>();
            text = GetComponentInChildren<RTLTMPro.RTLTextMeshPro>();

            if (text == null)
            {
                Debug.LogWarning("No Text Component Found on GrabHelper");
                return;
            }

            // Cache the initial scale
            initialScale = canvas.transform.localScale;

            // Set the initial scale to 0 (hidden)
            canvas.transform.localScale = Vector3.zero;

        }

        void Update()
        {
            AssignCamera();

            if (text == null || mainCam == null)
            {
                return;
            }


            var remoteGrababble =  leftGrabbablesInTrigger.ClosestRemoteGrabbable;
            var rightRemoteGrababble =            rightGrabbablesInTrigger.ClosestRemoteGrabbable;

         

            if (remoteGrababble != null && !leftGrabber.HeldGrabbable && leftGrabber.RemoteGrabbingGrabbable == null)
            {
                    ShowCanvas(remoteGrababble);
            }
            else if(rightRemoteGrababble != null && !righttGrabber.HeldGrabbable && righttGrabber.RemoteGrabbingGrabbable == null)
            {
               ShowCanvas(rightRemoteGrababble);
            }
            else
            {
                HideCanvas();
            }
            currentGrabbable = remoteGrababble;
        }

        void ShowCanvas(Grabbable newGrabbable)
        {
            if (newGrabbable != currentGrabbable)
            {
                text.text =  newGrabbable.GetComponent<Item>().id.ToString().Replace("_"," ");

                canvas.transform.localScale = Vector3.zero;
                isShowing = true;
                canvas.enabled = true; // Ensure the canvas is enabled
                Collider collider = newGrabbable.GetComponent<Collider>();
                canvas.transform.position = collider.bounds.center + Vector3.up * (collider.bounds.extents.y + upOffest);
                canvas.transform.DOScale(initialScale, tweenDuration).SetEase(Ease.OutBack);
            }
            canvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        void HideCanvas()
        {           
            canvas.enabled = false; // Disable canvas after scaling down to zero
            canvas.transform.localScale = initialScale; // Reset scale to cached initial scale   
        }

        public virtual void AssignCamera()
        {
            if (mainCam == null)
            {
                // Find By Tag instead of Camera.main as the camera could be disabled
                if (GameObject.FindGameObjectWithTag("MainCamera") != null)
                {
                    mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
                }
            }
        }


        bool test = false;
        [SerializeField] Grabbable TestGrabbable;
        [Button]
        public void TestCase() {
            test = !test;
        }
      
    }

  
}
