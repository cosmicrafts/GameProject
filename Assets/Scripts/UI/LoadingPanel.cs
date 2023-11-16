using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingPanel : MonoBehaviour
{
   
    
    private static LoadingPanel _instance; // field
    public static LoadingPanel Instance   // property
    {
        get
        {
            if (_instance == null) { _instance = Instantiate( ResourcesServices.LoadLoadingPanel() ).GetComponent<LoadingPanel>(); }
            return _instance;
        }   
        private set { _instance = value; }  
    }
    
 
    
    private Animator animator;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            //DesactiveLoadingPanel();
        }
        else
        {
            Destroy(gameObject);
        }
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

