using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.InteropServices;
public class UIMainMenu : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void JSMetaWallet(string walletID);


    [SerializeField]
    MyWallet metaMaskWallet;


    public static UIMainMenu Menu;
    
    public GameObject LoginPanel;
    public GameObject MenuPanel;
    public GameObject MatchPanel;
    public GameObject MultiPanel;

    public GameObject MainMenu;
    public GameObject CollectionMenu;
    public GameObject CharactersMenu;
    public GameObject GameModesMenu;

    public GameObject BackBtn;

    public UICollection Collection;

    public User PlayerUser;
    public UserProgress PlayerProgress;
    public UserCollection PlayerCollection;
    public NFTsCharacter PlayerCharacter;

    public Text CurrentGameMode;
    public Text CurrentGameModeStatus;

    //TimeSpan TimeMatch;
    //DateTime StartTime;

    public Text TopTitle;
    //public Image GameTitle;
    public Image LocalGameLoadingBar;

    List<UIPTxtInfo> UIPropertys;

    int UserDataLoaded;


    private void Awake()
    {
        Menu = this;
        UserDataLoaded = 0;

     //  string playerWalletRose = PlayerPrefs.GetString("AccounName");
    //   GL_SetPlayerData(playerWalletRose);

       
      //  LoginPanel.SetActive(true); el panel login
    
      // GoLoginPage();
        if (!GameData.DataReady)
        { 
            SaveData.LoadGameConfig();
            PlayerCollection = GameData.GetUserCollection();
            PlayerCollection.AddUnitsAndCharactersDefault();
            GameData.DataReady = true;
        }
        else
        {
            
            InitPlayerData();
        }
        CheckGameMode();

        GameData.DebugMode = false;

#if UNITY_EDITOR
        GameData.DebugMode = true;
#endif

#if UNITY_WEBGL
        GameData.CurrentPlataform = Plataform.Web;

#endif

        if (GameData.DebugMode)
        {
           
            GameData.CurrentMatch = Match.bots;
            InitPlayerData();
        }

        if(GameData.CurrentPlataform == Plataform.Web)
        {
           
            GameData.CurrentMatch = Match.tutorial;
            PlayTutorial();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        UIPropertys = new List<UIPTxtInfo>();
        foreach(UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>())
        {
            UIPropertys.Add(prop);
        }
        //TimeMatch = new TimeSpan(0, 0, 5);

        if (GameData.UserIsInit())
        {
            LoginPanel.SetActive(false);
            MenuPanel.SetActive(true);
            
            
        }


       GameNetwork.JSWalletStart();
    }
  
    

        public void GL_SetPlayerData(string jsonData)
    {
        User user = JsonConvert.DeserializeObject<User>(jsonData);
        GameData.SetUser(user);
        AddProgressDataLoaded();
    }

    public void GL_SetCharacterData(string jsonData)
    {
        NFTsCharacter character = JsonConvert.DeserializeObject<NFTsCharacter>(jsonData);
        GameData.SetUserCharacter(character.KeyId);
        AddProgressDataLoaded();
    }

    public void GL_SetProgressData(string jsonData)
    {
        Progress progress = JsonConvert.DeserializeObject<Progress>(jsonData);
        UserProgress userProgress = new UserProgress();
        userProgress.InitValues(progress);
        GameData.SetUserProgress(userProgress);
        AddProgressDataLoaded();
    }

    public void GL_SetConfigData(string jsonData)
    {
        Config config = JsonConvert.DeserializeObject<Config>(jsonData);
        GameData.SetConfig(config);
        GameData.ChangeLang((Language)config.language);
        AddProgressDataLoaded();
    }

    void AddProgressDataLoaded()
    {
        UserDataLoaded++;
        if (UserDataLoaded == 4)
            InitPlayerData();
    }

    void InitPlayerData()
    {
        PlayerUser = GameData.GetUserData();
        PlayerProgress = GameData.GetUserProgress();
        PlayerCharacter = GameData.GetUserCharacter();
        LoginPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    void PlayIA()
    {
        PlayerUser.FirstGame = false;

        MainMenu.SetActive(false);
        MatchPanel.SetActive(true);

        StartCoroutine(LoadLocalGame());
    }

    void PlayTutorial()
    {
        PlayerUser.FirstGame = true;

        MainMenu.SetActive(false);
        MatchPanel.SetActive(true);

        StartCoroutine(LoadLocalGame());
    }

    void PlayMulti()
    {
        PlayerUser.FirstGame = true;

        MultiPanel.GetComponent<UIMatchMaking>().StartSearch();
    }

    public void GoLoginPage()
    {
#if UNITY_EDITOR
        InitPlayerData();
#else
        Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/");
#endif
    }

    public void GoDiscordPage()
    {
        Application.OpenURL("http://discord.gg/cosmicrafts");
    }

    public void GoAirdropsPage()
    {
        Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/");
    }

    public void ChangeLang(int lang)
    {
        GameData.ChangeLang((Language)lang);
    }

    IEnumerator LoadLocalGame()
    {   
        AsyncOperation loading = SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);

        while(!loading.isDone)
        {
            yield return null;
            LocalGameLoadingBar.fillAmount = loading.progress;
        }
    }

    public void GoCollectionMenu()
    {
        MainMenu.SetActive(false);
        CollectionMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_chyur_deck");
        //GameTitle.gameObject.SetActive(false);
    }

    public void GoCharactersMenu()
    {
        MainMenu.SetActive(false);
        CharactersMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_chyur_char");
        //GameTitle.gameObject.SetActive(false);
    }

    public void GoGamesModesMenu()
    {
        MainMenu.SetActive(false);
        GameModesMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_gamemodes");
        //GameTitle.gameObject.SetActive(false);
    }

    public void PlayCurrentMode()
    {
        switch(GameData.CurrentMatch)
        {
            case Match.bots:
                {
                    PlayIA();
                }
                break;
            case Match.multi:
                {
                    PlayMulti();
                }
                break;
            case Match.tutorial:
                {
                    PlayTutorial();
                }
                break;
            default:
                {
                    PlayMulti();
                }
                break;
        }
    }

    public void BackMainSection()
    {
        RefreshAllPropertys();
        MainMenu.SetActive(true);
        CollectionMenu.SetActive(false);
        CharactersMenu.SetActive(false);
        GameModesMenu.SetActive(false);
        BackBtn.SetActive(false);
        TopTitle.text = string.Empty;
        //GameTitle.gameObject.SetActive(true);
    }

    public void ChangeCurrentGameMode(int newMode)
    {
        GameData.CurrentMatch = (Match)newMode;
        CheckGameMode();
        BackMainSection();
    }

    void CheckGameMode()
    {
        switch (GameData.CurrentMatch)
        {
            case Match.bots:
                {
                    CurrentGameMode.text = Lang.GetText("mn_pve");
                    CurrentGameModeStatus.text = string.Empty;
                }
                break;
            case Match.multi:
                {
                    CurrentGameMode.text = Lang.GetText("mn_pvp");
                    CurrentGameModeStatus.text = Lang.GetText("mn_unranked");
                }
                break;
            case Match.tutorial:
                {
                    CurrentGameMode.text = Lang.GetText("mn_tutorial");
                    CurrentGameModeStatus.text = string.Empty;
                }
                break;
            default:
                {
                    CurrentGameMode.text = Lang.GetText("mn_pvp");
                    CurrentGameModeStatus.text = Lang.GetText("mn_unranked");
                }
                break;
        }
    }

    public void RefreshProperty(PlayerProperty property)
    {
        foreach(UIPTxtInfo prop in UIPropertys.Where(f => f.Property == property))
        {
            prop.LoadProperty();
        }
    }

    public void RefreshAllPropertys()
    {
        foreach (UIPTxtInfo prop in UIPropertys)
        {
            prop.LoadProperty();
        }
    }
}
