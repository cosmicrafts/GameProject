using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyWallet : MonoBehaviour
{
    public Text walletID;
    public Text nickName;

    string walletAccountID;
    public string WalletAccountID => walletAccountID;

    private void Awake()
    {
        SetWalletID();
    }
    void Start()
    {
     

        if (nickName)
        {
            nickName.text = PlayerPrefs.GetString("AccounName");
        }
    }
   
    void SetWalletID()
    {
        if (PlayerPrefs.HasKey("Account")){

            walletAccountID = PlayerPrefs.GetString("Account");

            walletID.text = PlayerPrefs.GetString("Account");
        }


    }
   
    
}
