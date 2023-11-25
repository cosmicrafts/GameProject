using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candid;
using CanisterPK.CanisterMatchMaking.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class UIMatchLoading : MonoBehaviour
{
    [Header("UI Match and Stats from Game")]
    public GameObject MatchLoadingScreen;
    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;
    public Text Txt_VsLevel;
    public Image Img_VsIcon;
    public Image Img_VsEmblem;
    
    //Loading Bar (used when a new scene is loading)
    public Image LocalGameLoadingBar;

    public User MyUser = new User();
    public User VsUser = new User();


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    /*
    Debug.Log("MATCH STARTING");
        
    VsUser vsUser = JsonConvert.DeserializeObject<VsUser>(json);*/

    [System.Serializable]
    public class MatchPlayerData
    {
        public int userAvatar;
        public List<String> listSavedKeys;

    }
    public async void GL_MatchPreStarting()
    {

        Debug.Log("MATCH PRESTARTING");

        Txt_VsWalletId.text = "Loading";
        Txt_VsNikeName.text = "Loading";
        Txt_VsLevel.text = "Loading";
        
        var Characters = GlobalGameData.Instance.GetUserCollection().Characters;
        NFTsCharacter vsCharacter = Characters.FirstOrDefault(f=>f.ID == 0 );
        if (vsCharacter != null)
        {
            Img_VsIcon.sprite = vsCharacter.IconSprite;
            Img_VsEmblem.sprite = ResourcesServices.LoadCharacterEmblem(vsCharacter.KeyId);
        }
        
        MatchLoadingScreen.SetActive(true);
        //StartCoroutine(LoadLocalGame());
        
        //Obtener lista del deck para enviar al canister
        UserCollection.SavedKeyIds savedKeyIds = new UserCollection.SavedKeyIds();
        if (PlayerPrefs.HasKey("savedKeyIds")) { savedKeyIds = JsonUtility.FromJson<UserCollection.SavedKeyIds>(PlayerPrefs.GetString("savedKeyIds")); }
        List<String> listSavedKeys = new List<string>(); 
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Alliance){ listSavedKeys = savedKeyIds.AllSavedKeyIds; } 
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Spirats) { listSavedKeys = savedKeyIds.SpiSavedKeyIds; }
        if((Factions)GlobalGameData.Instance.GetUserCharacter().Faction == Factions.Webe)    { listSavedKeys = savedKeyIds.WebSavedKeyIds; }

        MatchPlayerData matchPlayerData = new MatchPlayerData();
        matchPlayerData.userAvatar = GlobalGameData.Instance.GetUserData().Avatar;
        matchPlayerData.listSavedKeys = listSavedKeys;

        Debug.Log(JsonUtility.ToJson(matchPlayerData));
        var AddMatchPlayerDataRequest = await CandidApiManager.Instance.CanisterMatchMaking.AddMatchPlayerData(JsonUtility.ToJson(matchPlayerData));

        if (AddMatchPlayerDataRequest)
        {
            Debug.Log("Data subida, llamando GetMachtData");
            GetMatchData();
        }
        else
        {
            Debug.Log("Ocurrio un error en AddMatchPlayerDataRequest");
        }
        
    }

    public async void GetMatchData()
    {
        
        var matchDataRequest = await CandidApiManager.Instance.CanisterMatchMaking.GetMyMatchData();
        
        if (matchDataRequest.Arg0.HasValue)
        {
            CanisterPK.CanisterMatchMaking.Models.MatchData matchData = matchDataRequest.Arg0.ValueOrDefault;

            User UserData1 = new User();
            User UserData2 = new User();

            CanisterPK.CanisterMatchMaking.Models.PlayerInfo tempData1 = new PlayerInfo();
            CanisterPK.CanisterMatchMaking.Models.PlayerInfo tempData2 = new PlayerInfo();

            tempData1 = matchData.Player1;
            if (matchData.Player2.HasValue) { tempData2 = matchData.Player2.ValueOrDefault; }

            if (tempData1.PlayerGameData.IsNullOrEmpty() || tempData2.PlayerGameData.IsNullOrEmpty() )
            {
                Debug.Log("Aun ambos usuarios no han subido la información, volviendo a consultar:");
                await Task.Delay(500);
                if (this.gameObject != null)
                {
                    GetMatchData();
                }
            }
            else
            {
                UserData1.WalletId = tempData1.Id.ToString();
                UserData1.NikeName = "Falta este valor";
                UserData1.Level = (int) tempData1.Elo;
                Debug.Log(tempData1.PlayerGameData);
                MatchPlayerData matchPlayerData1 = JsonUtility.FromJson<MatchPlayerData>(tempData1.PlayerGameData);
                UserData1.CharacterNFTId = matchPlayerData1.userAvatar;
                UserData1.DeckNFTsKeyIds = matchPlayerData1.listSavedKeys;
          
                UserData2.WalletId = tempData2.Id.ToString();
                UserData2.NikeName = "Falta este valor";
                UserData2.Level = (int) tempData2.Elo;
                Debug.Log(tempData2.PlayerGameData);
                MatchPlayerData matchPlayerData2 = JsonUtility.FromJson<MatchPlayerData>(tempData2.PlayerGameData);
                UserData2.CharacterNFTId = matchPlayerData2.userAvatar;
                UserData2.DeckNFTsKeyIds = matchPlayerData2.listSavedKeys;

                if ((int) matchDataRequest.Arg1 != 0)
                {
                    if ((int) matchDataRequest.Arg1 == 1)
                    {
                        GL_MatchStarting(UserData1, UserData2);
                    }
                    else if ((int) matchDataRequest.Arg1 == 2)
                    {
                        GL_MatchStarting(UserData2, UserData1);
                    }
                }
                
            }
            
            
        }
        else
        {
            Debug.Log("No hay info del match");
        }
        
    }
    
    public void GL_MatchStarting(User MyUserData, User VsUserData)
    {
        MyUser = MyUserData;
        VsUser = VsUserData;
        
        Debug.Log("MATCH STARTING");
        
        Txt_VsWalletId.text = VsUserData.WalletId;
        Txt_VsNikeName.text = VsUserData.NikeName;
        Txt_VsLevel.text = VsUserData.Level.ToString();
        
        var Characters = GlobalGameData.Instance.GetUserCollection().Characters;
        NFTsCharacter vsCharacter = Characters.FirstOrDefault(f=>f.ID == VsUserData.CharacterNFTId );
        if (vsCharacter != null)
        {
            Img_VsIcon.sprite = vsCharacter.IconSprite;
            Img_VsEmblem.sprite = ResourcesServices.LoadCharacterEmblem(vsCharacter.KeyId);
        }
        
       // SearchingScreen.SetActive(false);
        //AcceptMatchScreen.SetActive(false);
        MatchLoadingScreen.SetActive(true);
        StartCoroutine(LoadLocalGame());
        
    }
    
    IEnumerator LoadLocalGame()
    {
        yield return new WaitForSeconds(1f);
        
        AsyncOperation loading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        while (!loading.isDone)
        {
            yield return null;
            LocalGameLoadingBar.fillAmount = loading.progress;
        }
    }
    
    public void OnInitMatch()
    {
        Debug.Log("MATCH FINISH");
        
        Destroy(this.gameObject);
    }
    
    
}
