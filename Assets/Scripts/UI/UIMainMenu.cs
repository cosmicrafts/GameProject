using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Menu;

    public GameObject LoginPanel;
    public GameObject MenuPanel;
    public GameObject MatchPanel;
    public GameObject MultiPanel;

    public GameObject MainMenu;
    public GameObject CollectionMenu;
    public GameObject CharactersMenu;
    public GameObject GameModesMenu;

    public GameObject PlayerDataComp;
    public GameObject BackBtn;

    public UICollection Collection;

    public User PlayerUser;
    public UserProgress PlayerProgress;
    public UserCollection PlayerCollection;
    public NFTsCharacter PlayerCharacter;

    //TimeSpan TimeMatch;
    //DateTime StartTime;

    public Text TopTitle;
    public Image GameTitle;
    public Image LocalGameLoadingBar;

    List<UIPTxtInfo> UIPropertys;

    private void Awake()
    {
        Menu = this;

        SaveData.LoadGameConfig();

        LoginPanel.SetActive(true);
        MenuPanel.SetActive(false);

        GameData.DebugMode = false;
#if UNITY_EDITOR
        GameData.DebugMode = true;
#endif

        if (GameData.DebugMode)
        {
            PlayerUser = GameData.GetUserData();
            PlayerProgress = GameData.GetUserProgress();
            PlayerCollection = GameData.GetUserCollection();
            PlayerCollection.AddUnitsDefault();
            PlayerCharacter = GameData.GetUserCharacter();
            LoginPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.CurrentMatch = Match.none;
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
    }

    private void Update()
    {

    }

    public void SetPlayerData(string jsonData)
    {
        PlayerUser = JsonConvert.DeserializeObject<User>(jsonData);
        GameData.SetUser(PlayerUser);
        PlayerProgress = GameData.GetUserProgress();
        PlayerCollection = GameData.GetUserCollection();
        PlayerCollection.AddUnitsDefault();
        PlayerCharacter = GameData.GetUserCharacter();
        LoginPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void PlayIAButton()
    {
        if (GameData.CurrentMatch != Match.none)
        {
            return;
        }

        PlayerUser.FirstGame = false;
        GameData.CurrentMatch = Match.bots;
        
        MenuPanel.SetActive(false);
        MatchPanel.SetActive(true);

        StartCoroutine(LoadLocalGame());
    }

    public void PlayTutorialButton()
    {
        if (GameData.CurrentMatch != Match.none)
        {
            return;
        }

        PlayerUser.FirstGame = true;
        GameData.CurrentMatch = Match.tutorial;

        MenuPanel.SetActive(false);
        MatchPanel.SetActive(true);

        StartCoroutine(LoadLocalGame());
    }

    public void PlayMultiButton()
    {
        if (GameData.CurrentMatch != Match.none)
        {
            return;
        }

        PlayerUser.FirstGame = true;
        GameData.CurrentMatch = Match.multi;

        MenuPanel.SetActive(false);
        MultiPanel.SetActive(true);
        MultiPanel.GetComponent<UIMatchMaking>().StartSearch();
    }

    public void GoLoginPage()
    {
#if UNITY_EDITOR
        User user = new User() { NikeName = "Player", Avatar = 1 };
        SetPlayerData(JsonConvert.SerializeObject(user));
#else
        Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/");
#endif
    }

    public void ChangeLang(int lang)
    {
        GameData.ChangeLang((Language)lang);
    }

    IEnumerator LoadLocalGame()
    {   
        AsyncOperation loading = SceneManager.LoadSceneAsync(1);

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
        TopTitle.text = Lang.GetText("mn_collection");
        GameTitle.gameObject.SetActive(false);
        PlayerDataComp.SetActive(false);
    }

    public void GoCharactersMenu()
    {
        MainMenu.SetActive(false);
        CharactersMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_characters");
        GameTitle.gameObject.SetActive(false);
        PlayerDataComp.SetActive(false);
    }

    public void GoGamesModesMenu()
    {
        MainMenu.SetActive(false);
        GameModesMenu.SetActive(true);
        BackBtn.SetActive(true);
        TopTitle.text = Lang.GetText("mn_gamemodes");
        GameTitle.gameObject.SetActive(false);
        PlayerDataComp.SetActive(false);
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
        GameTitle.gameObject.SetActive(true);
        PlayerDataComp.SetActive(true);
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
