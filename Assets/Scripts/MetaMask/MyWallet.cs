using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyWallet : MonoBehaviour
{
    [SerializeField]
Text walletID;
    [SerializeField]
    Text playerAccountName;
    // Start is called before the first frame update
    void Start()
    {
    
        walletID.text = PlayerPrefs.GetString("Account");

        if (playerAccountName)
            playerAccountName.text = PlayerPrefs.GetString("AccounName"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
