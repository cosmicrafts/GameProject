using System;
using System.Collections;
using System.Collections.Generic;
using Boom;
using Boom.Patterns.Broadcasts;
using Boom.Values;
using Candid;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class Login : MonoBehaviour
{
    public TMP_InputField inputNameField;
    public TMP_Text infoTxt;
    string mainScene = "Menu";
    
    [SerializeField] private Animator chooseLoginAnim;
    [SerializeField] private Animator chooseUserAnim;

    private void Awake()
    {
        BroadcastState.Register<CanLogin>(UpdateWindow, true);
        UserUtil.RegisterToLoginDataChange(UpdateWindow, true);
    }
    private void OnDestroy()
    {
        LoginManager.Instance.CancelLogin();
        UserUtil.UnregisterToLoginDataChange(UpdateWindow);
        BroadcastState.Unregister<CanLogin>(UpdateWindow);
    }
    
    private void UpdateWindow(CanLogin state)
    {
       //Apagar boton de login
       // logInBtn.interactable = state.value;
    }
    private void UpdateWindow(DataState<LoginData> state)
    {
        bool isLoading = state.IsLoading();
        var getIsLoginResult = UserUtil.GetLogInType();

        if (getIsLoginResult.Tag == UResultTag.Ok)
        {
            if(getIsLoginResult.AsOk() == UserUtil.LoginType.User)
            {
                Debug.Log("Logged In");
                Debug.Log($"Principal: <b>\"{state.data.principal}\"</b>\nAccountId: <b>\"{state.data.accountIdentifier}\"</b>");
                UserLoginSuccessfull();
            }
            else//Logged In As Anon
            {
                Debug.Log("Logged in as Anon");
                Debug.Log($"Principal: <b>\"{state.data.principal}\"</b>\nAccountId: <b>\"{state.data.accountIdentifier}\"</b>");
            }
        }
        else
        {
            if (isLoading) Debug.Log($"Loading Data...Loading");
            else  Debug.Log($"Error Not Loading Data...Loading");
        }
    }


    public void StartWebLogin()
    {
        LoadingPanel.Instance.ActiveLoadingPanel();
        chooseLoginAnim.Play("ChooseLogin_Outro");
        
        UserUtil.StartLogin(); //codigo de ICP+NFID
    }
    
    public async void UserLoginSuccessfull()
    { 
        var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();

        if (playerInfo.HasValue)
        {
            CanisterPK.CanisterLogin.Models.Player player = playerInfo.ValueOrDefault;
            Debug.Log(" ID: " + player.Id + " Lv: "+player.Level + " Name: " + player.Name);
            GoToMenuScene();
        }
        else
        {
            Debug.Log("Player 2 Dont has value");
            LoadingPanel.Instance.DesactiveLoadingPanel();
            chooseUserAnim.Play("ChooseUsername_Intro");
        }
    }

    public void GoToMenuScene()
    {
        LoadingPanel.Instance.ActiveLoadingPanel();
        SceneManager.LoadScene(mainScene);
    }
    
    public async void SetPlayerName()
    {
        if (inputNameField.text != null)
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            var request =  await CandidApiManager.Instance.CanisterLogin.CreatePlayer(inputNameField.text);
            if (request.Arg0)
            {
                Debug.Log(request.Arg1);
                GoToMenuScene();
            }
            else
            {
                infoTxt.text = request.Arg1;
                LoadingPanel.Instance.DesactiveLoadingPanel();
            }
            
        }
    }
    
    
    public void BackLoginMenu()
    {
        chooseUserAnim.Play("ChooseUsername_Outro"); 
        chooseLoginAnim.Play("ChooseLogin_Intro");
    }

    

    
   

}
