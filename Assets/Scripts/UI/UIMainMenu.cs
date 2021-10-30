using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class UIMainMenu : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject MenuPanel;
    public GameObject MatchPanel;

    public User PlayerUser;
    public UserProgress PlayerProgress;
    public UserCollection PlayerCollection;
    public NFTsCharacter PlayerCharacter;

    //TimeSpan TimeMatch;
    //DateTime StartTime;

    public Text StartCountDownText;
    public Image LocalGameLoadingBar;

    private void Awake()
    {
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
            PlayerCharacter = GameData.GetUserCharacter();
            LoginPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.CurrentMatch = Match.none;
        //TimeMatch = new TimeSpan(0, 0, 5);

        if (GameData.UserIsInit())
        {
            LoginPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameData.CurrentMatch == Match.multi)
        {
            //StartCountDownText.text = TimeMatch.Add(StartTime - DateTime.Now).ToString(@"%s");
        }
    }

    public void SetPlayerData(string jsonData)
    {
        PlayerUser = JsonConvert.DeserializeObject<User>(jsonData);
        GameData.SetUser(PlayerUser);
        LoginPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void PlayButton()
    {
        if (GameData.CurrentMatch != Match.none)
        {
            return;
        }

        PlayerUser.FirstGame = false;
        GameData.CurrentMatch = Match.multi;
        
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

    public void GoLoginPage()
    {
        Application.OpenURL("https://4nxsr-yyaaa-aaaaj-aaboq-cai.ic0.app/");
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
}
