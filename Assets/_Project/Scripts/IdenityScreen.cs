using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdenityScreen : MonoBehaviour
{

    [SerializeField] private AvatarManager _avatarManager;
    // Start is called before the first frame update
    void Start()
    {

        Invoke("InvokFunction", 1f);
    }



    private void InvokFunction()
    {
        _avatarManager.PlayStep("identity");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
