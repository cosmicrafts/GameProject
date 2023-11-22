using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Candid;
using CanisterPK.CanisterMatchMaking.Models;
using EdjCase.ICP.Candid.Models;
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

    public UIMatchLoading UIMatchLoading;
    
    
    public Text Txt_Tips;

    //Player data
    User MyUserData;
    User VsUserData;

    //The current serching was canceled
    bool IsCanceled;

    //Game start count down
    int CoutDown = 10;
    
    [System.Serializable]
    public class VsUser
    {
        public string walletID = string.Empty;
        public string nikename = string.Empty;
        public int level = 0;
        public int characterId = 0;
    }
    
    //Start searching for a match
    public async void StartSearch()
    {
        //Shown the corresponding UI
        btnGameModes.interactable = false;
        StatusGame.text = Lang.GetText("mn_matchmaking");
        SearchingScreen.SetActive(true);
        AcceptMatchScreen.SetActive(false);
        
        
        var matchSearchingInfo = await CandidApiManager.Instance.CanisterMatchMaking.GetMatchSearching();

        if (matchSearchingInfo.Arg0 == SearchStatus.Available)
        {
            var matchAssignInfo = await CandidApiManager.Instance.CanisterMatchMaking.AssignPlayer2(matchSearchingInfo.Arg1);

            if (matchAssignInfo.Arg0) { GL_MatchFound(); }
            else { StartSearch(); }
        }
        
        else if (matchSearchingInfo.Arg0 == SearchStatus.NotAvailable)
        {
            //Crear sala/partida
            var createdRoom = await CandidApiManager.Instance.CanisterMatchMaking.AddPlayerSearching();

            if (createdRoom.Arg0)
            {
                bool isGameMatched = false;

                //Verificar si IsGameMatched, en un bucle recursivo
                while (!isGameMatched)
                {
                    var isGameMatchedRequest = await CandidApiManager.Instance.CanisterMatchMaking.IsGameMatched();
                    isGameMatched = isGameMatchedRequest.Arg0;
                    await Task.Delay(500);
                }
                
                GL_MatchFound();
            }
        }
        else if(matchSearchingInfo.Arg0 == SearchStatus.Assigned)
        {
            bool isGameMatched = false;

            //Verificar si IsGameMatched, en un bucle recursivo
            while (!isGameMatched)
            {
                var isGameMatchedRequest = await CandidApiManager.Instance.CanisterMatchMaking.IsGameMatched();
                isGameMatched = isGameMatchedRequest.Arg0;
                await Task.Delay(500);
            }
                
            GL_MatchFound();
        }
        
    }
    
    public async void CancelSearch()
    {
        var cancelMatchmaking = await CandidApiManager.Instance.CanisterMatchMaking.CancelMatchmaking();
        if (cancelMatchmaking.Arg0)
        {
            SearchingScreen.SetActive(false);
            btnGameModes.interactable = true;
        }
    }
    
    public void GL_MatchFound()
    {
        SearchingScreen.SetActive(false);
        AcceptMatchScreen.SetActive(true);
        StartCoroutine(WaitingForAccept());
    }
    
    public async void AcceptMatch()
    {
        btn_Accept.SetActive(false);
        txt_WaitingPlayer.SetActive(true);
        StopCoroutine(WaitingForAccept());
        
        //Obtener lista del deck para enviar al canister
        UserCollection.SavedKeyIds savedKeyIds = new UserCollection.SavedKeyIds();
        if (PlayerPrefs.HasKey("savedKeyIds")) { savedKeyIds = JsonUtility.FromJson<UserCollection.SavedKeyIds>(PlayerPrefs.GetString("savedKeyIds")); }
        List<String> listSavedKeys = new List<string>(); 
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Alliance){ listSavedKeys = savedKeyIds.AllSavedKeyIds; } 
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Spirats) { listSavedKeys = savedKeyIds.SpiSavedKeyIds; }
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Webe)    { listSavedKeys = savedKeyIds.WebSavedKeyIds; }
        
        var acceptGame = await CandidApiManager.Instance.CanisterMatchMaking.AcceptMatch((UnboundedUInt)GlobalGameData.Instance.GetUserData().Avatar, listSavedKeys);

        if (acceptGame.Arg0)
        {
            bool isGameAccepted = false;

            //Verificar si IsGameMatched, en un bucle recursivo
            while (!isGameAccepted)
            {
                var isGameAcceptedRequest = await CandidApiManager.Instance.CanisterMatchMaking.IsGameAccepted();

                int isGameAcceptedCase = (int)isGameAcceptedRequest.Arg0;
               
                /*(0, "Game not accepted yet") (1, "Game accepted")
                    (2, "Other player rejected the game")(3, "Game not found for this player")*/
                switch (isGameAcceptedCase)
                {
                    case 0: break;
                    
                    case 1: 
                        isGameAccepted = true;  
                        var matchDataRequest = await CandidApiManager.Instance.CanisterMatchMaking.GetMyMatchData();
                        
                        if (matchDataRequest.Arg0.HasValue)
                        {
                            CanisterPK.CanisterMatchMaking.Models.MatchData matchData = matchDataRequest.Arg0.ValueOrDefault;
                            
                            User VsUserData = new User();
                            User MyUserData = GlobalGameData.Instance.GetUserData();

                            CanisterPK.CanisterMatchMaking.Models.PlayerInfo tempPlayer2 = new PlayerInfo();
                           
                            if (matchData.Player2.HasValue) { tempPlayer2 = matchData.Player2.ValueOrDefault; }
                            
                            if (matchDataRequest.Arg1 == 1)
                            {
                                MyUserData.DeckNFTsKeyIds = matchData.Player1.DeckSavedKeyIds;
                                
                                VsUserData.WalletId = tempPlayer2.Id.ToString();
                                VsUserData.NikeName = "Falta este valor";
                                VsUserData.Level = (int) tempPlayer2.Elo;
                                VsUserData.CharacterNFTId = (int)tempPlayer2.CharacterSelected;
                                VsUserData.DeckNFTsKeyIds = tempPlayer2.DeckSavedKeyIds;
                            }
                            else if(matchDataRequest.Arg1 == 2)
                            {
                                MyUserData.DeckNFTsKeyIds = tempPlayer2.DeckSavedKeyIds;
                                
                                VsUserData.WalletId = matchData.Player1.Id.ToString();
                                VsUserData.NikeName = "Falta este valor";
                                VsUserData.Level = (int) matchData.Player1.Elo;
                                VsUserData.CharacterNFTId = (int)matchData.Player1.CharacterSelected;
                                VsUserData.DeckNFTsKeyIds = matchData.Player1.DeckSavedKeyIds;
                            }
                            UIMatchLoading.GL_MatchStarting(MyUserData, VsUserData);
                        }
                        else
                        {
                            //No hay Mathf Info
                        }
                        
                        break;
                    
                    case 2: 
                        isGameAccepted = true;
                        StartSearch();
                        break;
                    case 3: 
                        isGameAccepted = true;
                        StartSearch();
                        break;
                        
                }
                
                await Task.Delay(500);
            }
        }
        else
        {
            
        }
       
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
        RejectMatch();
    }
    public async void RejectMatch()
    {
        SearchingScreen.SetActive(false);
        var rejectMatch = await CandidApiManager.Instance.CanisterMatchMaking.RejectMatch();
    }
    
    
    
    
    
   

    

}
