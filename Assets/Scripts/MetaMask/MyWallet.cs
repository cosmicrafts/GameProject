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
    // Start is called before the first frame update
    private void Awake()
    {
        walletID.text = PlayerPrefs.GetString("Account");
    }
    void Start()
    {
      

        SetWalletID();



        if (nickName)
        {
            nickName.text = PlayerPrefs.GetString("AccounName");
        }
    }
   
    void SetWalletID()
    {
        if (PlayerPrefs.HasKey("Account")){

            walletAccountID = PlayerPrefs.GetString("Account");
        }


    }
   
    
}
