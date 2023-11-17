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
    
    [Header("UI Match and Stats from Game")]
    public GameObject MatchScreen;
    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;
    public Text Txt_VsLevel;
    public Image Img_VsIcon;
    public Image Img_VsEmblem;
    
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
        MatchScreen.SetActive(false);
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
        MatchScreen.SetActive(false);
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
        MatchScreen.SetActive(false);
        //JS_RejectMatch();
    }
    public void GL_UserAcceptedButNotCouple()
    {
        StartSearch();
    }
    public void GL_MatchStarting(string json)
    {
        Debug.Log("MATCH STARTING");
        VsUser vsUser = JsonConvert.DeserializeObject<VsUser>(json);
        
        Txt_VsWalletId.text = vsUser.walletID;
        Txt_VsNikeName.text = vsUser.nikename;
        Txt_VsLevel.text = vsUser.level.ToString();
        
        var Characters = GlobalGameData.Instance.GetUserCollection().Characters;
        NFTsCharacter vsCharacter = Characters.FirstOrDefault(f=>f.ID == vsUser.characterId );
        if (vsCharacter != null)
        {
            Img_VsIcon.sprite = vsCharacter.IconSprite;
            Img_VsEmblem.sprite = ResourcesServices.LoadCharacterEmblem(vsCharacter.KeyId);
        }
        
        SearchingScreen.SetActive(false);
        AcceptMatchScreen.SetActive(false);
        MatchScreen.SetActive(true);
    }
    
    public void GL_FinishMatch()
    {
        Debug.Log("MATCH FINISH");
        
        SearchingScreen.SetActive(false);
        AcceptMatchScreen.SetActive(false);
        MatchScreen.SetActive(false);
    }

    

}
