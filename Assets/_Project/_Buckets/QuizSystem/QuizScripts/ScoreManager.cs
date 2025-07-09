using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{

    private static float _score = 0f;


    public static float Score
    {
        get { return _score; }
        set { _score = value;}
    }







    public static void ResetAllDatat()
    {

        _score = 0f;
    }


}
