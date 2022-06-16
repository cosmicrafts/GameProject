using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class Login : MonoBehaviour
{

    public InputField inputNameField;
    [SerializeField]
    GameObject namePanel;
    string playerName;

    private string account;
    string mainScene = "Menu";

    [SerializeField]
    GameObject LoginPanel;

    [SerializeField]
    GameObject loadingPanel;
    private Animator anim;


    // Start is called before the first frame update


    void CloseLogin()
    {
        if (anim != null)
        {
            anim.Play("Close_PanelLogin");
        }
    }
    void ClosePanelLoading()
    {
        if (anim != null)
        {
            anim.Play("Close_PanelLoading");
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
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

        LoginPanel.SetActive(true);
    }

    public void OnRecibeNameData(string usser)//esta es de PK
    {
        if (string.IsNullOrEmpty(usser))
        {

            LoginPanel.SetActive(false);
            namePanel.SetActive(true);
        }
        else
        {
            LoadingPanel.instance.ActiveLoadingPanel();
            SceneManager.LoadScene(mainScene);

        }

    }


    public void OnNameData(int usser)//esta es de PK
    {
        if (usser == 1)
        {
            LoadingPanel.instance.ActiveLoadingPanel();
            SceneManager.LoadScene(mainScene);
            //tutorial
        }
        else

        {
            LoadingPanel.instance.DesactiveLoadingPanel();
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
            LoginPanel.SetActive(true);
        }

    }

    public void StoickLogin()
    {
        LoadingPanel.instance.ActiveLoadingPanel();
        GameNetwork.JSWalletsLogin("stoicWallet");
       
    }
    public void IdentityLogin()
    {
        LoadingPanel.instance.ActiveLoadingPanel();
        GameNetwork.JSWalletsLogin("identityWallet");
       
    }

}
