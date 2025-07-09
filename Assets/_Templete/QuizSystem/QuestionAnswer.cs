using BNG;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

namespace QuizSystem
{
    public class QuestionAnswer : MonoBehaviour
    {
        public AnswerEnum answer;

        [Tooltip("Used only for ORDER Question Answers")]
        public TMP_Text order;
        [ReadOnly] public GameObject grabbableInZone; //Not used for ID questions
        [ReadOnly] public SnapZone snapZone;
        [Tooltip("Used only for ORDER Question Answers")]
        [ReadOnly] public Button pushButton; 
        [ReadOnly] public bool ButtonClicked = false; // used for validation
        //[ReadOnly] public UnityEngine.UI.Button uiButton;
        private void Start()
        {
            SetAllOptions();

            AssignGrabbableEvent();
            AssignClickEventToButton();
            AssignUISnapZoneEvent();

            FixLayers();
        }

        private void FixLayers()
        {
            switch (PlatformManager.Instance.platform)
            {
                case Platform.Webgl:
                    if (snapZone)
                    {
                        int layer = LayerMask.NameToLayer("Default");
                        gameObject.layer = layer;
                    }
                break;
                case Platform.Android:
                    if (snapZone)
                    {
                        int layer = LayerMask.NameToLayer("Ignore Raycast");
                        gameObject.layer = layer;
                    }
                break;
            }
        }
        private void SetAllOptions()
        {
            snapZone = GetComponent<SnapZone>();
            pushButton = GetComponent<Button>();

        }
        /// <summary>
        /// Used to determine whether the snapzone has an object or not
        /// </summary>
        public void AssignGrabbableEvent()
        {
            if (snapZone != null)
            {
                snapZone.OnSnapEvent.AddListener(AssignGrabbable);
                snapZone.OnDetachEvent.AddListener(UnAssignGrabbable);
            }
        }

        /// <summary>
        /// Used to determine whether a button has been clicked or not
        /// </summary>
        public void AssignClickEventToButton()
        {
            if (pushButton != null)
            {
                pushButton.onButtonDown.AddListener(AssignClick);
            }
        }
        /// <summary>
        /// Used to determine whether the UI snapzone has an object or not
        /// </summary>
        public void AssignUISnapZoneEvent()
        {
            UIDropZone dropZone = GetComponent<UIDropZone>();

            if (dropZone != null)
            {
                dropZone.OnSnapEvent.AddListener(AssignGrabbable);
                dropZone.OnDetachEvent.AddListener(UnAssignGrabbable);
            }
        }
        private void AssignClick()
        {
            ButtonClicked = true;
        }


        //used for 3d snapzone
        public void AssignGrabbable(Grabbable grab)
        {
            grabbableInZone = grab.gameObject;
        }
        public void UnAssignGrabbable(Grabbable grab)
        {
            grabbableInZone = null;
        }
        //used for 2d snapzone
        public void AssignGrabbable(GameObject grab)
        {
            grabbableInZone = grab;
        }
        public void UnAssignGrabbable(GameObject grab)
        {
            grabbableInZone = null;
        }
        public bool HasAnswer()
        {
            if (grabbableInZone != null) return true;
            if (ButtonClicked) return true;

            return false;
        }
        private void OnMouseDown()
        {
            //Debug.Log(" CALLED ");
                if (pushButton != null) transform.DOLocalMoveY(transform.localPosition.y - 0.5f, 0.3f);
                if (snapZone != null) ChooseCurrentChoice();            
        }
        public void ChooseCurrentChoice()
        {
            if (grabbableInZone != null)
            {
                grabbableInZone.GetComponent<Collider>().enabled = true;
                grabbableInZone.GetComponent<RestOnUnGrab>().ForceDefaultPosition();
                grabbableInZone = null;
            }
            if (ChoiceSnapManager.Instance.CurrentChoice == null && grabbableInZone == null) return;
            ChoiceSnapManager.Instance.CurrentChoice.gameObject.transform.DOMove(transform.position, 0.8f).OnComplete(() =>
            {
                ChoiceSnapManager.Instance.CurrentChoice.gameObject.GetComponent<Collider>().enabled = false;
                grabbableInZone = ChoiceSnapManager.Instance.CurrentChoice.gameObject;


                ChoiceSnapManager.Instance.CurrentChoice = null;
                snapZone.OnSnapEvent.Invoke(grabbableInZone.GetComponent<CustomGrabbable>());
            });


        }
    }
}

