using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using Mono.CompilerServices.SymbolWriter;
using TMPro;
using UnityEngine.Networking;



/*
 * This is the UI Main menu controller
 * Manages the UI references and windows to navigate through the game menus and start the in-game scenes
 * Also recive the back-end functions to initialize the player data
 */

public class UIMainMenu : MonoBehaviour
{
    //Self public reference
    public static UIMainMenu Instance;

    //Main sections
    public GameObject MenuPanel;
    public GameObject LoadingMatchPanel;
    
    
    //Sub sections
    public GameObject MainMenu;
    public GameObject Persistent;
    public GameObject CollectionMenu;
    public GameObject CharactersMenu;
    public GameObject GameModesMenu;
        public GameObject GameModes_Modes;
        public GameObject GameModes_Enemies;
        
    //Back button 
    public GameObject BackBtn;

    //UI Collection controller
    public UICollection Collection;

    //User data
    public UserCollection PlayerCollection;

    //public UserTokens PlayerTokens; //New user data for cryptocurrencies

    public bool defaultPrefabs = true;
    public List<ShipsDataBase> ShipsDataBasesAlliance;
    public List<ShipsDataBase> ShipsDataBasesSpirats;
    public List<ShipsDataBase> ShipsDataBasesWebe;
    
    //UI Text references for game mode and game status
    public TMP_Text CurrentGameMode;
    public Image CurrentImageGameMode;
    public TMP_Text CurrentGameModeStatus;

    //Current section title
    public TMP_Text TopTitle;

    

    //UI User data references
    List<UIPTxtInfo> UIPropertys;

    //Progress of the loaded user data
    int UserDataLoaded;
    int targetCharacterId;

    //public Dropdown botMode;
    public Dropdown botDificulty;

    public GameModeCard[] gamemodes;
    public UIMatchMaking uiMatchMaking;
    
   private void Awake()
   {
       
       //initialize variables
        Instance = this;
        UserDataLoaded = 0;
       
        //Find and save all the UI player properties
        UIPropertys = new List<UIPTxtInfo>();
        foreach (UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>()) { UIPropertys.Add(prop); }

        //Hide Menu
        MenuPanel.SetActive(false);
        //Init Player Collection
        PlayerCollection = GlobalGameData.Instance.GetUserCollection();

        //If the essential data doesn't exist...
        if (!GlobalGameData.Instance.DataReady)
        {
            Debug.Log("Load Game Config");
            //Initialize the essential data
            SaveData.LoadGameConfig();
            
        }
        else
        {
            Debug.Log("INIT PLAYER DATA");
            //Load the player data
            InitPlayerData();
        }

        if (defaultPrefabs)
        {
            InitPlayerData();
        }
        /*#if UNITY_EDITOR
        InitPlayerData();
        #endif*/

        Debug.Log("CHECK GAMES MODES");
        //Check the current game mode
        CheckGameMode();

       
    }

 
    //Called from WEB, for set the base player data
    public void GetUserData(string jsonData)
    {
        User user = JsonConvert.DeserializeObject<User>(jsonData);
        GlobalGameData.Instance.SetUser(user);
        AddProgressDataLoaded();
    }
    
    //Called from WEB, for set the base player progress
    public void GL_SetProgressData(string jsonData)
    {
        Debug.Log("GL SET PROGRESS DATA");
        Progress progress = JsonConvert.DeserializeObject<Progress>(jsonData);
        UserProgress userProgress = new UserProgress();
        userProgress.InitValues(progress);
        GlobalGameData.Instance.SetUserProgress(userProgress);
        AddProgressDataLoaded();
    }

    //Called from WEB, for set the base player config
    public void GL_SetConfigData(string jsonData)
    {
        Debug.Log("GL SET CONFIG DATA");
        Config config = JsonConvert.DeserializeObject<Config>(jsonData);
        GlobalGameData.Instance.SetConfig(config);
        GlobalGameData.Instance.ChangeLang((Language)config.language);
        AddProgressDataLoaded();
    }

    //Called from WEB, for set the player characters collection
    public void GL_SetCollectionCharactersData(string jsonData)
    {
        Debug.Log("GL SET CHARACTERS DATA");
        PlayerCollection.SetCharacters(jsonData);
        Debug.Log(jsonData);
    }

    //Called from WEB, for set the player Units collection
    public void GL_SetCollectionUnitsData(string jsonData)
    {
        Debug.Log("GL SET COLLECTION DATA");
        PlayerCollection.SetUnitCards(jsonData);
    }

    //Called from WEB, for set the player Spells collection
    public void GL_SetCollectionSkillsData(string jsonData)
    {
        PlayerCollection.SetSpellsCards(jsonData);
    }
    
   
    

    //Add progress of the player loaded data
    void AddProgressDataLoaded()
    {
        UserDataLoaded++;
        if (UserDataLoaded == 4)
            InitPlayerData();
    }

    //Load the player´s data
    void InitPlayerData()
    {
       
        /*if (GlobalManager.GMD.DebugMode)*/ 
        if(defaultPrefabs && PlayerCollection.Characters.Count == 0 && PlayerCollection.Cards.Count == 0)
        {
            PlayerCollection.AddUnitsAndCharactersDefault(ShipsDataBasesAlliance, ShipsDataBasesSpirats, ShipsDataBasesWebe);
        }
        //Init Deck
        PlayerCollection.InitDecks();
        
        //Set Character
        GlobalGameData.Instance.SetUserCharacter(GlobalGameData.Instance.GetConfig().characterSavedID );
        
        //Load icons
        StartCoroutine(LoadNFTsIcons());
        
        LoadingPanel.Instance.DesactiveLoadingPanel();
        //Start Menu        
        GlobalGameData.Instance.DataReady = true;
       
        MenuPanel.SetActive(true);
    }

    //Start the IA game mode
    void PlayIA()
    {
        int dificulty = botDificulty.value;
        PlayerPrefs.SetInt("Dificulty", dificulty);
        MainMenu.SetActive(false);
        LoadingMatchPanel.SetActive(true);
        
    }
    public void ChangeModeGame(GameModeCard gameModeCard)
    {
        PlayerPrefs.SetInt("BotMode", gameModeCard.idMode);
        CurrentImageGameMode.sprite = gameModeCard.imageMode.sprite;
        CurrentGameMode.text = gameModeCard.nameMode.text;
        
        GlobalGameData.Instance.SetCurrentMatch(gameModeCard.match);
        
        Persistent.SetActive(true);
        MainMenu.SetActive(true);
        GameModesMenu.SetActive(false);
        
    }

    //Update the UI for the current game mode
    void CheckGameMode()
    {
        if(PlayerPrefs.HasKey("BotMode")){
            int currentBotMode = PlayerPrefs.GetInt("BotMode");
            CurrentImageGameMode.sprite = gamemodes[currentBotMode].imageMode.sprite;
            CurrentGameMode.text = gamemodes[currentBotMode].nameMode.text;
        }
    }
    
 
    public void GoDiscordPage() { Application.OpenURL("http://discord.gg/cosmicrafts"); }
    public void GoAirdropsPage() { Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/"); }
    
    public void ChangeLang(int lang)
    {
        GlobalGameData.Instance.ChangeLang((Language)lang);
        RefreshAllPropertys();
    }

    //Load the game scene for a Tutorial o PVIA game
    IEnumerator LoadLocalGame()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        while (!loading.isDone)
        {
            yield return null;
            //LocalGameLoadingBar.fillAmount = loading.progress;
        }
    }

    

    //Show the games mode menu
    public void GoGamesModesMenu()
    {
        MainMenu.SetActive(false);
        GameModesMenu.SetActive(true);
            GameModes_Modes.SetActive(true);
            GameModes_Enemies.SetActive(false);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_gamemodes");
    }

    //Start the current game mode
    public void PlayCurrentMode()
    {
        switch (GlobalGameData.Instance.GetConfig().currentMatch)
        {
            case Match.multi:
            {
                uiMatchMaking.StartSearch();
                break;
            }
            case Match.bots:
            {
                Persistent.SetActive(false);
                Debug.Log($"CURRENT MATCH: {GlobalGameData.Instance.GetConfig().currentMatch}");
                PlayIA();
                break;
            }
            default:
            {
                PlayIA();
                break;
            }
                
        }
    }

    //Go back to the main menu
    public void BackMainSection()
    {
        RefreshAllPropertys();
        MainMenu.SetActive(true);
        CollectionMenu.SetActive(false);
        CharactersMenu.SetActive(false);
        GameModesMenu.SetActive(false);
        BackBtn.SetActive(false);
        TopTitle.text = string.Empty;
    }


    //Refresh a specific UI propertie of the player
    public void RefreshProperty(PlayerProperty property)
    {
        foreach (UIPTxtInfo prop in UIPropertys.Where(f => f.Property == property)) { prop.LoadProperty(); }
    }
    //Refresh all the UI references properties of the player
    public void RefreshAllPropertys()
    {
        foreach (UIPTxtInfo prop in UIPropertys) { prop.LoadProperty(); }
    }

    //Load NFTs Icons
    private IEnumerator LoadNFTsIcons()
    {
        //LOAD URL CHARACTERS ICONS
        foreach (NFTsCharacter character in PlayerCollection.Characters)
        {
            if (!string.IsNullOrEmpty(character.IconURL) && character.IconSprite == null)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(character.IconURL);
                
                yield return www.SendWebRequest();
                Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                character.IconSprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }
        //LOAD URL CARDS ICONS
        foreach (NFTsCard card in PlayerCollection.Cards)
        {
            if (!string.IsNullOrEmpty(card.IconURL) && card.IconSprite == null)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(card.IconURL);
                yield return www.SendWebRequest();
                Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                card.IconSprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }

        //Refresh Cards Icons
        foreach (UICard card in FindObjectsOfType<UICard>())
        {
            card.RefreshIcon();
        }

        RefreshAllPropertys();
    }
}
