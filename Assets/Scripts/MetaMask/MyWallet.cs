using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyWallet : MonoBehaviour
{
    public Text walletID;

    // Start is called before the first frame update
    void Start()
    {
        walletID.text = PlayerPrefs.GetString("Account");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
