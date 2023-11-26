using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Candid;
using CanisterPK.CanisterMatchMaking.Models;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class NetworkRPC : PunRPC { }

public class PunNetworkManager : MonoBehaviourPunCallbacks
{
    public bool getInfoFromCanister = false;
    public GameManager GameManager;
    public NetworkMessenger Messenger;
    public static PunNetworkManager NetworkManager;
    private PhotonView view;
    public TMP_Text lobbyTxt;

    public string nameRoom = "Cosmicrafts_Room0";
    private bool connectedToGame = false;
    
    public bool IsMaster {
        get { return PhotonNetwork.IsMasterClient; }
    }
    public int PlayerIndex
    {
        get {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[i].UserId) { return i; }
                }
                return 0;
        }
    }

    private void Awake()
    {
        if (NetworkManager && NetworkManager != this)
        {
            Destroy(gameObject);
            return;
        }
        if (!NetworkManager)
        {
            NetworkManager = this;
            view = this.gameObject.AddComponent<PhotonView>();
            view.ViewID = 1;
            view.ObservedComponents = new List<Component>(0)
            {
                Messenger
            };
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator Start()
    {
        if (getInfoFromCanister)
        {
            GetInfoFromCanister();
        }
       
        while(true)
        {
            UpdateConnecting();
            Debug.Log("UpdateConnecting");
            yield return new WaitForSeconds(1.0f);
        }
    }

    public async void GetInfoFromCanister()
    {
         var matchDataRequest = await CandidApiManager.Instance.CanisterMatchMaking.GetMyMatchData();
                        
        if (matchDataRequest.Arg0.HasValue)
        {
            CanisterPK.CanisterMatchMaking.Models.MatchData matchData = matchDataRequest.Arg0.ValueOrDefault;
            nameRoom = "Game: " + matchData.GameId;
            
            User UserData1 = new User();
            User UserData2 = new User();

            CanisterPK.CanisterMatchMaking.Models.PlayerInfo tempData1 = new PlayerInfo();
            CanisterPK.CanisterMatchMaking.Models.PlayerInfo tempData2 = new PlayerInfo();

                                              tempData1 = matchData.Player1;
            if (matchData.Player2.HasValue) { tempData2 = matchData.Player2.ValueOrDefault; }

            UserData1.WalletId = tempData1.Id.ToString();
            UserData1.NikeName = "Falta este valor";
            UserData1.Level = (int) tempData1.Elo;
            UIMatchLoading.MatchPlayerData matchPlayerData1 = JsonUtility.FromJson<UIMatchLoading.MatchPlayerData>(tempData1.PlayerGameData);
            UserData1.CharacterNFTId = matchPlayerData1.userAvatar;
            UserData1.DeckNFTsKeyIds = matchPlayerData1.listSavedKeys;
          
            UserData2.WalletId = tempData2.Id.ToString();
            UserData2.NikeName = "Falta este valor";
            UserData2.Level = (int) tempData2.Elo;
            UIMatchLoading.MatchPlayerData matchPlayerData2 = JsonUtility.FromJson<UIMatchLoading.MatchPlayerData>(tempData2.PlayerGameData);
            UserData2.CharacterNFTId = matchPlayerData2.userAvatar;
            UserData2.DeckNFTsKeyIds = matchPlayerData2.listSavedKeys;

            GameManager.UserData1 = UserData1;
            GameManager.UserData2 = UserData2;
            GameManager.updateHeroesPrefab = true;
            if ((int) matchDataRequest.Arg1 != 0) {  GameManager.GroupIndex = (int)matchDataRequest.Arg1;  }
            
        }
        else
        {
            //No hay Mathf Info
        }
        
    }

    public void RPCToOthers(string methodName, params object[] parameters)
    {
        view.RPC(methodName, RpcTarget.Others, parameters);
    }

    private void UpdateConnecting()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("LoadLobby");
            ReconnectingGame();
        }
        else if (!connectedToGame && Application.internetReachability != NetworkReachability.NotReachable && !PhotonNetwork.IsConnected )
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = "Player_" + (PhotonNetwork.CountOfPlayersInRooms + 1) + "_" + UnityEngine.Random.Range(1, 1000);
        Debug.Log("Connected To Master");
        if (!PhotonNetwork.InLobby) { PhotonNetwork.JoinLobby(); }
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(nameRoom, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        PhotonNetwork.CurrentRoom.IsVisible = true;
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length == 2) { LoadGame(); }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
        LoadGame();
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed " + message);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Messenger.ClearActions();
        ReconnectingGame();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        ReconnectingGame();
    }


    private void LoadGame()
    {
        //Iniciar el Juego aqui
        lobbyTxt.text = "InGame";
        connectedToGame = true;
        GameManager.Pause(false);
        GameManager.Init();
    }
    private void ReconnectingGame()
    {
        lobbyTxt.text = "Loading or Connecting";
        connectedToGame = false;
        GameManager.Pause(true);
    }
    public void ExitGame()
    {
        Messenger.ClearActions();
        PhotonNetwork.Disconnect();
    }
    
}
