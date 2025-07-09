using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HintEngine : MonoSinglton<HintEngine>
{

    public  HintMotion hintMotion;
    public  HintMotion[] hintMotions;

    public HandPoser poser;

    public void SetActiveMotion(int index) {
      this.hintMotion = hintMotions[index];
    }

    [Button]
    public void ActiveHint() { 
      if(hintMotion != null) {
         hintMotion.Activate(poser);  
      }
    }

}


[System.Serializable]
public class HintMotion {

    [TextArea(3,10)] public string name;  

    [Space] public UnityEvent2 OnStart;
    [Space] public UnityEvent2 OnDone;

    public Transform source;
    public Transform target;
    public GameObject targetObjct;
    // public GameObject modle;

    public HandPose handPose;

    internal bool active = false;

    [SerializeField] float _upOffest = 0.15f;

    [SerializeField] float snapTime = 3f;


    private HandPoser handPoser;


    public void Activate(HandPoser poser)
    {
        if (active) 
            return;
        else
        {
            StartMotion(poser);
            handPoser = poser;
        }
    }

    public void RestMotion() { 
      handPoser.gameObject.SetActive(false);
      targetObjct.gameObject.SetActive(true);
    }

    private void StartMotion(HandPoser poser)
    {       
        var grabPoints  = source.GetComponentsInChildren<GrabPoint>();
        for (int i = 0; i < grabPoints.Length; i++) {
            if (grabPoints[i].RightHandIsValid) {
                poser.transform.parent = grabPoints[i].transform;
                poser.CurrentPose = handPose;
                poser.transform.rotation = grabPoints[i].transform.rotation;
                poser.transform.position = grabPoints[i].transform.position;
                poser.gameObject.SetActive(true);
                targetObjct.transform.parent = poser.transform;
                targetObjct.gameObject.SetActive(true);
            } 
        }

        active = true;

        Vector3[] points = 
            GetPath(target.position,  poser.transform.position);
     
        OnStart?.Invoke();       
        
        poser.transform.DOPath(points, snapTime * 2.5f).OnComplete(() =>
        {
            active = false;
            OnDone?.Invoke();
        });
    }

    private Vector3[] GetPath(Vector3 position,Vector3 objectPoistion)
    {
        float higgerY = position.y < objectPoistion.y ? objectPoistion.y : position.y;

        Vector3[] points = new Vector3[4];
        Vector3 point1 = objectPoistion;
        point1.y = higgerY;

        points[0] = objectPoistion;
        //0
        points[1] = point1 + Vector3.up * _upOffest;

        Vector3 point2 = position;
        point2.y = higgerY;

        //1
        points[2] = point2 + Vector3.up * (_upOffest);

        //2
        points[3] = position;

        return points;
    }

}