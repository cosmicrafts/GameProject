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
    public UIMatchLoading UIMatchLoading;

    //Player data
    User MyUserData;
    User VsUserData;

    private bool sendPlayerActive = false;
        
    
    [System.Serializable]
    public class VsUser
    {
        public string walletID = string.Empty;
        public string nikename = string.Empty;
        public int level = 0;
        public int characterId = 0;
    }
    [System.Serializable]
    public class MatchPlayerData
    {
        public int userAvatar;
        public List<String> listSavedKeys;
    }
    
    //Start searching for a match
    public async void StartSearch()
    {
        //Shown the corresponding UI
        btnGameModes.interactable = false;
        StatusGame.text = Lang.GetText("mn_matchmaking");
        SearchingScreen.SetActive(true);
        
        //Obtener lista del deck para enviar al canister
        UserCollection.SavedKeyIds savedKeyIds = new UserCollection.SavedKeyIds();
        if (PlayerPrefs.HasKey("savedKeyIds")) { savedKeyIds = JsonUtility.FromJson<UserCollection.SavedKeyIds>(PlayerPrefs.GetString("savedKeyIds")); }
        List<String> listSavedKeys = new List<string>(); 
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Alliance){ listSavedKeys = savedKeyIds.AllSavedKeyIds; } 
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Spirats) { listSavedKeys = savedKeyIds.SpiSavedKeyIds; }
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Webe)    { listSavedKeys = savedKeyIds.WebSavedKeyIds; }

        MatchPlayerData matchPlayerData = new MatchPlayerData();
        matchPlayerData.userAvatar = GlobalGameData.Instance.GetConfig().characterSavedID;
        matchPlayerData.listSavedKeys = listSavedKeys;

        Debug.Log(JsonUtility.ToJson(matchPlayerData));

        var matchSearchingInfo = await CandidApiManager.Instance.CanisterMatchMaking.GetMatchSearching(JsonUtility.ToJson(matchPlayerData));
        
        Debug.Log("Status: "+ matchSearchingInfo.ReturnArg0 + " Int: " + matchSearchingInfo.ReturnArg1 + " text: " + matchSearchingInfo.ReturnArg2);
        
        if(matchSearchingInfo.ReturnArg0 == SearchStatus.Assigned)
        {
            bool isGameMatched = false;
            sendPlayerActive = true; SendPlayerActive();
            //Verificar si IsGameMatched, en un bucle recursivo
            while (!isGameMatched && SearchingScreen.activeSelf)
            {
                if(this.gameObject == null) { break; }
                var isGameMatchedRequest = await CandidApiManager.Instance.CanisterMatchMaking.IsGameMatched();
                Debug.Log("Ya estoy asignado a una sala: " + matchSearchingInfo.ReturnArg1 +" espero ser matched: " + isGameMatchedRequest.ReturnArg1);
                isGameMatched = isGameMatchedRequest.ReturnArg0;
                await Task.Delay(500);
                
                if(isGameMatched) { sendPlayerActive = false; GL_MatchFound(); }
            }
                
            
        }
        
    }

    private async void SendPlayerActive()
    {
        while (sendPlayerActive && SearchingScreen.activeSelf)
        {
            if(this.gameObject == null) { break; }
            var isActive = await CandidApiManager.Instance.CanisterMatchMaking.SetPlayerActive();
            Debug.Log("estoy activo: "+ isActive);

            if (!isActive)
            {
                CancelSearch();
                break;
            }
            
            await Task.Delay(2000);
        }
    }

    public async void CancelSearch()
    {
        var cancelMatchmaking = await CandidApiManager.Instance.CanisterMatchMaking.CancelMatchmaking();
        Debug.Log("Quiero Cancelar la busqueda: " + cancelMatchmaking.ReturnArg1);
        if (cancelMatchmaking.ReturnArg0)
        {
            SearchingScreen.SetActive(false);
            btnGameModes.interactable = true;
        }
    }
    
    public void GL_MatchFound()
    {
        SearchingScreen.SetActive(false);
        UIMatchLoading.GL_MatchPreStarting();
    }
    
    
    
    
   

    

}
