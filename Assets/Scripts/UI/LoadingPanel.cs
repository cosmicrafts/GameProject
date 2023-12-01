using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingPanel : MonoBehaviour
{
    private Animator animator;
    private static LoadingPanel _instance;
    public static LoadingPanel Instance {
        get 
        {
            if (_instance == null) { _instance = Instantiate( ResourcesServices.LoadLoadingPanel() ).GetComponent<LoadingPanel>(); }
            return _instance;
        }
    }
    private void Awake() {
        animator = this.GetComponent<Animator>();
        if (!_instance) { _instance = this; DontDestroyOnLoad(gameObject); } //DesactiveLoadingPanel();
        else { Destroy(gameObject); }
    }
    
    public void ActiveLoadingPanel()
    {
        animator.Play("OnChain_transaction");
    }
    public void DesactiveLoadingPanel()
    {
        animator.Play("OnChain_transaction_finished");
    }
    
}

