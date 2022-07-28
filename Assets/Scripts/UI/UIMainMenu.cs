using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.Networking;

/*
 * This is the UI Main menu controller
 * Manages the UI references and windows to navigate through the game menus and start the in-game scenes
 * Also recive the back-end functions to initialize the player data
 */

public class UIMainMenu : MonoBehaviour
{
    //Self public reference
    public static UIMainMenu Menu;

    //Main sections
    //  public GameObject LoginPanel;
    public GameObject MenuPanel;
    public GameObject MatchPanel;
    public GameObject MultiPanel;

    //Sub sections
    public GameObject MainMenu;
    public GameObject CollectionMenu;
    public GameObject CharactersMenu;
    public GameObject GameModesMenu;
    // Intro Door

    [SerializeField]
    Animator doorAnim;
    //Back button 
    public GameObject BackBtn;

    //UI Collection controller
    public UICollection Collection;

    //User data
    public User PlayerUser;
    public UserProgress PlayerProgress;
    public UserCollection PlayerCollection;
    public NFTsCharacter PlayerCharacter;

    //UI Text references for game mode and game status
    public Text CurrentGameMode;
    public Text CurrentGameModeStatus;

    //Current section title
    public Text TopTitle;

    //Loading Bar (used when a new scene is loading)
    public Image LocalGameLoadingBar;

    //UI User data references
    List<UIPTxtInfo> UIPropertys;

    //Progress of the loaded user data
    int UserDataLoaded;
    int isFirstLogin =0;
    public GameObject welcomePanel;
    private void Awake()
    {

        //LoadingPanel.instance.DesactiveLoadingPanel();
        Debug.Log("--- MENU START ---");
        //Instanciate Global Manager
        if (FindObjectOfType<GlobalManager>() == null)
        {
            Instantiate(ResourcesServices.LoadGlobalManager());
        }
        //initialize variables
        Menu = this;
        UserDataLoaded = 0;
        //Find and save all the UI player properties
        UIPropertys = new List<UIPTxtInfo>();
        foreach (UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>())
        {
            UIPropertys.Add(prop);
        }

        //Show the login page
        //LoadingPanel.instance.ActiveLoadingPanel();

        MenuPanel.SetActive(false);

        //If the essential data doesn't exist...
        if (!GlobalManager.GMD.DataReady)
        {
            //Initialize the essential data
            SaveData.LoadGameConfig();
            //Load the player NFTs collection
            PlayerCollection = GlobalManager.GMD.GetUserCollection();
            //Add default NFTs if we are debuging
            if (GlobalManager.GMD.DebugMode)
                PlayerCollection.AddUnitsAndCharactersDefault();
        }
        else
        {
            //Load the player data
            InitPlayerData();
        }

        //Check the current game mode
        CheckGameMode();

        //If this is a debug runtime...
        if (GlobalManager.GMD.DebugMode)
        {
            //Set the game mode as bots
            GlobalManager.GMD.CurrentMatch = Match.bots;
            //Set the default test units

            //Load the player data (with default vaules)
            InitPlayerData();
        }
        Debug.Log("--- MENU REDY ---");

    }

    // Start is called before the first frame update
    void Start()
    {
        //Check if we already have the user data
        if (GlobalManager.GMD.UserIsInit())
        {
            //Show the main menu

            //LoginPanel.SetActive(false);
         

            MenuPanel.SetActive(true);

            if (!PlayerPrefs.HasKey("firstLogin"))
            {

                if (isFirstLogin == 0)
                {

                    welcomePanel.SetActive(true);
                    isFirstLogin = 1;
                    PlayerPrefs.SetInt("firstLogin", 1);
                }
                else
                {
                    welcomePanel.SetActive(false);
                }
            }
            PlayerPrefs.GetInt("firstLogin");
        }

    }

    //Called from WEB, for set the base player data
    public void GL_SetPlayerData(string jsonData)
    {
        User user = JsonConvert.DeserializeObject<User>(jsonData);
        GlobalManager.GMD.SetUser(user);
        AddProgressDataLoaded();
    }

    //Called from WEB, for set the player character
    public void GL_SetCharacterSelected(int NFTid)
    {
        GlobalManager.GMD.SetUserCharacter(NFTid);
        AddProgressDataLoaded();
    }

    //Called from WEB, for set the base player progress
    public void GL_SetProgressData(string jsonData)
    {
        Progress progress = JsonConvert.DeserializeObject<Progress>(jsonData);
        UserProgress userProgress = new UserProgress();
        userProgress.InitValues(progress);
        GlobalManager.GMD.SetUserProgress(userProgress);
        AddProgressDataLoaded();
    }

    //Called from WEB, for set the base player config
    public void GL_SetConfigData(string jsonData)
    {
        Config config = JsonConvert.DeserializeObject<Config>(jsonData);
        GlobalManager.GMD.SetConfig(config);
        GlobalManager.GMD.ChangeLang((Language)config.language);
        AddProgressDataLoaded();
    }

    //Called from WEB, for set the player characters collection
    public void GL_SetCollectionCharactersData(string jsonData)
    {
        PlayerCollection.SetCharacters(jsonData);
    }

    //Called from WEB, for set the player Units collection
    public void GL_SetCollectionUnitsData(string jsonData)
    {
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
        Debug.Log("--- MENU SHOW ---");
        PlayerUser = GlobalManager.GMD.GetUserData();
        PlayerProgress = GlobalManager.GMD.GetUserProgress();
        PlayerCharacter = GlobalManager.GMD.GetUserCharacter();
        PlayerCollection.InitDecks();
        StartCoroutine(LoadNFTsIcons());
        if (!GlobalManager.GMD.DebugMode)
            LoadingPanel.instance.DesactiveLoadingPanel();
        doorAnim.SetTrigger("DoorIntro");
        MenuPanel.SetActive(true);
    }

    //Start the IA game mode
    void PlayIA()
    {
        PlayerUser.FirstGame = false;

        MainMenu.SetActive(false);
        MatchPanel.SetActive(true);

        StartCoroutine(LoadLocalGame());
    }

    //Start the Tutorial
    void PlayTutorial()
    {
        PlayerUser.FirstGame = true;

        MainMenu.SetActive(false);
        MatchPanel.SetActive(true);

        StartCoroutine(LoadLocalGame());
    }

    //Serch for a multiplayer match
    void PlayMulti()
    {
        PlayerUser.FirstGame = true;

        MultiPanel.GetComponent<UIMatchMaking>().StartSearch();
    }

    //Redirect to the login page
    public void GoLoginPage()
    {
#if UNITY_EDITOR
        InitPlayerData();
#else
        Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/");
#endif
    }

    //Open the cosmicrafts discord
    public void GoDiscordPage()
    {
        Application.OpenURL("http://discord.gg/cosmicrafts");
    }

    //Open the airdrip page
    public void GoAirdropsPage()
    {
        Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/");
    }

    //Change the game language
    public void ChangeLang(int lang)
    {
        GlobalManager.GMD.ChangeLang((Language)lang);
    }
    public void OnClaimWelcomeNft(int nftIndex)
    {
        GameNetwork.JSClaimNft(nftIndex);
       welcomePanel.SetActive(false);
    }
    //Load the game scene for a Tutorial o PVIA game
    IEnumerator LoadLocalGame()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        while (!loading.isDone)
        {
            yield return null;
            LocalGameLoadingBar.fillAmount = loading.progress;
        }
    }

    //Show the collection menu
    public void GoCollectionMenu()
    {
        MainMenu.SetActive(false);
        CollectionMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_chyur_deck");
        //GameTitle.gameObject.SetActive(false);
    }

    //Show the characters menu
    public void GoCharactersMenu()
    {
        MainMenu.SetActive(false);
        CharactersMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_chyur_char");
        //GameTitle.gameObject.SetActive(false);
    }

    //Show the games mode menu
    public void GoGamesModesMenu()
    {
        MainMenu.SetActive(false);
        GameModesMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_gamemodes");
        //GameTitle.gameObject.SetActive(false);
    }

    //Start the current game mode
    public void PlayCurrentMode()
    {
        Debug.Log("-- PLAY --");
        switch (GlobalManager.GMD.CurrentMatch)
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
        //GameTitle.gameObject.SetActive(true);
    }

    //Change the current game mode
    public void ChangeCurrentGameMode(int newMode)
    {
        GlobalManager.GMD.CurrentMatch = (Match)newMode;
        CheckGameMode();
        BackMainSection();
    }

    //Update the UI for the current game mode
    void CheckGameMode()
    {
        switch (GlobalManager.GMD.CurrentMatch)
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

    //Refresh a specific UI propertie of the player
    public void RefreshProperty(PlayerProperty property)
    {
        foreach (UIPTxtInfo prop in UIPropertys.Where(f => f.Property == property))
        {
            prop.LoadProperty();
        }
    }

    //Refresh all the UI references properties of the player
    public void RefreshAllPropertys()
    {
        foreach (UIPTxtInfo prop in UIPropertys)
        {
            prop.LoadProperty();
        }
    }

    //Load NFTs Icons
    private IEnumerator LoadNFTsIcons()
    {
        Debug.Log("LOADING NFT ICONS");
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
