using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchMaking : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SearchingScreen;
    public GameObject MatchScreen;

    WaitForSeconds DeltaOne;
    WaitForSeconds DeltaThree;

    public GameObject CancelButton;
    public GameObject GameFinded;
    public GameObject SearchingFor;

    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;

    User MyUserData;
    User VsUserData;
    bool IsCanceled;

    // Start is called before the first frame update
    void Start()
    {
        DeltaOne = new WaitForSeconds(1f);
        DeltaThree = new WaitForSeconds(3f);
        MyUserData = GameData.GetUserData();
    }


    public void StartSearch()
    {
        SearchingScreen.SetActive(true);
        MatchScreen.SetActive(false);
        CancelButton.SetActive(true);
        GameFinded.SetActive(false);
        SearchingFor.SetActive(true);
        GameData.MatchId = 0;
        IsCanceled = false;
        StartCoroutine(Searching());
    }

    public void CancelSearch()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
        IsCanceled = true;
        StopAllCoroutines();
    }

    void UpdateVsData()
    {
        VsUserData = GameNetwork.GetVsData();
        Txt_VsWalletId.text = VsUserData.WalletId;
        Txt_VsNikeName.text = VsUserData.NikeName;
    }

    IEnumerator Searching()
    {
        //Find Match
        GameData.ImMaster = false;
        GameData.MatchId = GameNetwork.JSSearchGame(MyUserData.WalletId);
        yield return DeltaOne;

        if (IsCanceled)
            yield return null;
        //Check for results
        if (GameData.MatchId == 0)
        {
            //New Match
            StartCoroutine(Creating());
            yield return null;
        }

        //Join Match
        Debug.Log($"Match: {GameData.MatchId}");
        CancelButton.SetActive(false);
        SearchingFor.SetActive(false);
        GameFinded.SetActive(true);

        string json = GameNetwork.JSGetGameData(GameData.MatchId);
        GameNetwork.UpdateGameData(json);
        GameNetwork.SetClientData(MyUserData.WalletId, MyUserData.NikeName);
        GameNetwork.JSSendGameData(GameNetwork.GetJsonGameNetPack(), GameData.MatchId);
        UpdateVsData();
        //Wait For Ready

        yield return DeltaThree;
        json = GameNetwork.JSGetGameData(GameData.MatchId);
        GameNetwork.UpdateGameData(json);
        if (GameNetwork.GetGameStatus() == NetGameStep.Ready)
        {
            SearchingScreen.SetActive(false);
            MatchScreen.SetActive(true);
        }
    }

    IEnumerator Creating()
    {
        GameData.ImMaster = true;
        GameNetwork.InitGameNetwork();
        GameNetwork.SetMasterData(MyUserData.WalletId, MyUserData.NikeName);
        GameData.MatchId = GameNetwork.JSCreateGame(MyUserData.WalletId);
        yield return new WaitUntil(() => GameData.MatchId != 0);
        GameNetwork.SetGameStatus(NetGameStep.Searching);
        GameNetwork.JSSendGameData(GameNetwork.GetJsonGameNetPack(), GameData.MatchId);

        bool fullRoom = false;
        while (!fullRoom)
        {
            yield return DeltaOne;
            string json = GameNetwork.JSGetGameData(GameData.MatchId);
            GameNetwork.UpdateGameData(json);
            fullRoom = GameNetwork.GameRoomIsFull();
        }
        if (IsCanceled)
            yield return null;

        Debug.Log($"Match: {GameData.MatchId}");
        CancelButton.SetActive(false);
        SearchingFor.SetActive(false);
        GameFinded.SetActive(true);

        GameNetwork.SetGameStatus(NetGameStep.Ready);
        GameNetwork.JSSendGameData(GameNetwork.GetJsonGameNetPack(), GameData.MatchId);

        SearchingScreen.SetActive(false);
        MatchScreen.SetActive(true);
        UpdateVsData();
    }
}
