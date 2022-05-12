using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WebGLSendTransactionExample : MonoBehaviour
{
    [SerializeField]
    string testAccount= "0xf0d056015Bdd86C0EFD07000F75Ea10873A1d0A7";
    [SerializeField]
    Text hash;
    async public void OnSendTransaction()
    {
        // account to send to
        string to = testAccount;
        // amount in wei to send
        string value = "1000000000000000000";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to send a transaction
        try {
            string response = await Web3GL.SendTransaction(to, value, gasLimit, gasPrice);
            Debug.Log(response);
            hash.text = response;
        } catch (Exception e) {
            Debug.LogException(e, this);
        }
    }

    public void Update()
    {
        if (hash.text.Contains("0"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
