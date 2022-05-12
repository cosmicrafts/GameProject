using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);
    public GameObject namePanel;
    private int expirationTime;
    private string account;
    bool haveAccount =false;
    [SerializeField]
    InputField inputNameField;
    [SerializeField]
    string mainScene;
    string playerName;
    private void Awake()
    {
        namePanel.SetActive(false);
    }
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
    
        GameNetwork.JSMetaWallet(account);
        // reset login message
        SetConnectAccount("");
    
    
     
        // load next scene
      
    }
   // SceneManager.LoadScene(2);
    public void OnRecibeMetaMaskData(string usser)//esta es de PK
    {
        if (PlayerPrefs.HasKey("AccounName"))
        {
            SceneManager.LoadScene(mainScene);
        }
        else
        {

                namePanel.SetActive(true);
            

        }
        
    }
    public void OnMetaNameData(int usser)//esta es de PK
    {
        if (usser == 1)
        {
            SceneManager.LoadScene(mainScene);
            //tutorial
        }
        else

        {
            //Nombre no valido
        }

    }
    public void SetPlayerName()
    {
        if (inputNameField.text != null)
        {
            playerName = inputNameField.text;

            PlayerPrefs.SetString("AccounName", playerName);


            GameNetwork.JSMetaUsserName(playerName);
        }
    
  
    }
   

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 

    public bool HaveUsser()
    {
        return haveAccount;
    }


}

