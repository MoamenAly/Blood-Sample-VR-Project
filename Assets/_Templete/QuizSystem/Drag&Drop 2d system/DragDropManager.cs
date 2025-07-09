using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class DragDropManager : MonoBehaviour
    {
        static public DragDropManager instance;
        [ReadOnly][SerializeField] private List<DragDropUI> DragDropClicked;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
#if UNITY_ANDROID
    public void CheckOnPreviousDraggables(DragDropUI clickedNow)
        {
            foreach(DragDropUI clicked in DragDropClicked)
            {
                if(clicked != clickedNow && clicked.isSelected) clicked.Release();
            }
        //DragDropClicked.ForEach(clicked => Debug.Log(clicked.gameObject.name + "  " + clicked.isSelected ));
            DragDropClicked.RemoveAll( clicked => !clicked.isSelected ); //only keep selected
        }

        public void AssignSelectedDraggable(DragDropUI clicked)
        {

        if (!DragDropClicked.Contains(clicked)) 
        {
            DragDropClicked.Add(clicked);
            CheckOnPreviousDraggables(clicked);
        } 

           
        }
#endif
    }



