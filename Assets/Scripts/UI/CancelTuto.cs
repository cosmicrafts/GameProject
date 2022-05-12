using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CancelTuto : MonoBehaviour
{
    public static CancelTuto instance;
    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this);
    }
    public static bool fristGame;

    public  void OnCancelTutorial()
    {
        fristGame = false;

    }

}
