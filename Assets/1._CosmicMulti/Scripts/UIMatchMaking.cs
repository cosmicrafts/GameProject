using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMatchMaking : MonoBehaviour
{
    public Button btnGameModes;
    public Text StatusGame;
    
    [Header("UI Search")]
    public GameObject SearchingScreen;
    
    [Header("UI AcceptMatch")]
    public GameObject AcceptMatchScreen;
    public GameObject btn_Accept;
    public GameObject txt_WaitingPlayer;
    public TMP_Text   tmp_Countdown;
    
    
    
    public Text Txt_Tips;

    //Player data
    User MyUserData;
    User VsUserData;

    //The current serching was canceled
    bool IsCanceled;

    //Game start count down
    int CoutDown;
    
    [System.Serializable]
    public class VsUser
    {
        public string walletID = string.Empty;
        public string nikename = string.Empty;
        public int level = 0;
        public int characterId = 0;
    }
    
    //Start searching for a match
    public void StartSearch()
    {
        //Shown the corresponding UI
        btnGameModes.interactable = false;
        StatusGame.text = Lang.GetText("mn_matchmaking");
        SearchingScreen.SetActive(true);
        AcceptMatchScreen.SetActive(false);
        //JS_SearchGame();
    }
    
    public void CancelSearch()
    {
        SearchingScreen.SetActive(false);
        btnGameModes.interactable = true;
       // JS_CancelSearchGame();
    }
    
    public void GL_MatchFound()
    {
        SearchingScreen.SetActive(false);
        AcceptMatchScreen.SetActive(true);
        StartCoroutine(WaitingForAccept());
    }
    
    public void AcceptMatch()
    {
        btn_Accept.SetActive(false);
        txt_WaitingPlayer.SetActive(true);
        StopCoroutine(WaitingForAccept());
        //JS_AcceptMatch(PlayerPrefs.GetInt("CharacterSaved"));
    }
    
    IEnumerator WaitingForAccept()
    {
        float duration = CoutDown; 
        while(duration > 0)
        {
            duration = duration - Time.deltaTime;
            tmp_Countdown.text = duration.ToString(CultureInfo.CurrentCulture);
            yield return null;
        }
        SearchingScreen.SetActive(false);
        //JS_RejectMatch();
    }
    public void GL_UserAcceptedButNotCouple()
    {
        StartSearch();
    }
    
    
   

    

}
