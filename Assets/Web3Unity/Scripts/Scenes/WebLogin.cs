using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;
    bool haveAccount =false;
    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        PlayerPrefs.SetString("Account", account); //esto guarda la cartera


        // reset login message
        SetConnectAccount("");
    
      /*  if (haveAccount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
   */
     
        // load next scene
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void Update()
    {
      
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
  /*  public void UserWalletEVMCall()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if(haveAccount)
       
#endif
    }
    public void StartWallet(bool usser)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
          
     if(!usser) {

     haveAccount=false;
    }else{
   haveAccount =true;

        JSWalletStart();
         }
#endif
    }*/

    public bool HaveUsser()
    {
        return haveAccount;
    }


}

