using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OasisWallet : MonoBehaviour
{
    public Text walletId;
    void Start()
    {
        walletId.text = PlayerPrefs.GetString("Account");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
