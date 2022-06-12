using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoginTest : MonoBehaviour
{
    
    public InputField inputNameField;
    [SerializeField]
    GameObject namePanel;
    string playerName;
    private string account;
    string mainScene="MainScene";
    bool haveAccount = false;
    string blockChainName;
    [SerializeField]
    GameObject LoginPanel;

    void Start()
    {
          namePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnConnectToWallet()
    {

        GameNetwork.JSLoginPanel(account); //esto aca se la manda a pk
        // reset login message
      

    }

    public void OnRecibeNameData(string usser)//esta es de PK
    {
        if (string.IsNullOrEmpty(usser))
        {
            namePanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(mainScene);

        }

    }
   

    public void OnNameData(int usser)//esta es de PK
    {
        if (usser == 1)
        {
            SceneManager.LoadScene(mainScene);
            //tutorial
        }
        else

        {
            LoginPanel.SetActive(false);
            namePanel.SetActive(true);
        }

    }

    public void SetPlayerName()
    {
     if (inputNameField.text != null)
     {
         playerName = inputNameField.text;
         PlayerPrefs.SetString("AccounName", playerName);
         GameNetwork.JSLoginPanel(playerName);
     }
 
 }

    public void StoickLogin()
    {
     GameNetwork.JSWalletsLogin("stoicWallet");
        LoginPanel.SetActive(true);
    }
    public void IdentityLogin()
    {
        GameNetwork.JSWalletsLogin("identityWallet");
        LoginPanel.SetActive(true);
    }

}
