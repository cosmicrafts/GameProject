using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMatchMaking : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SearchingScreen;
    public GameObject MatchScreen;

    WaitForSeconds DeltaOne;

    public GameObject CancelButton;
    public GameObject GameFinded;
    public GameObject SearchingFor;

    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;

    public Text Txt_CountDown;

    User MyUserData;
    User VsUserData;
    bool IsCanceled;

    int CoutDown;

    // Start is called before the first frame update
    void Start()
    {
        DeltaOne = new WaitForSeconds(1f);
        MyUserData = GameData.GetUserData();
    }


    public void StartSearch()
    {
        SearchingScreen.SetActive(true);
        MatchScreen.SetActive(false);
        CancelButton.SetActive(true);
        GameFinded.SetActive(false);
        SearchingFor.SetActive(true);
        GameNetwork.Start();
        IsCanceled = false;
        CoutDown = 5;
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
        Txt_VsWalletId.text = Utils.GetWalletIDShort(VsUserData.WalletId);
        Txt_VsNikeName.text = VsUserData.NikeName;
    }

    public void GLGetICPData(string json)
    {
        if (IsCanceled)
            return;
        Debug.Log(json);
        GameNetwork.UpdateGameData(json);
    }

    IEnumerator Searching()
    {
        //Find Match
        GameData.ImMaster = false;
        GameNetwork.JSSearchGame(MyUserData.WalletId);

        //Wait for match
        yield return new WaitUntil(() => GameNetwork.GetId() != 0);

        if (IsCanceled)
            yield return null;

        //Join Match
        Debug.Log($"Match: {GameNetwork.GetId()}");
        CancelButton.SetActive(false);
        SearchingFor.SetActive(false);
        GameFinded.SetActive(true);

        //Wait for ready
        yield return new WaitUntil(() => GameNetwork.GameRoomIsFull());

        UpdateVsData();
        SearchingScreen.SetActive(false);
        MatchScreen.SetActive(true);
        Txt_CountDown.text = CoutDown.ToString();

        AsyncOperation loading = SceneManager.LoadSceneAsync(1);
        loading.allowSceneActivation = false;
        //Final Count Down
        yield return DeltaOne;

        while (CoutDown > 0)
        {
            CoutDown--;
            Txt_CountDown.text = CoutDown.ToString();
            yield return DeltaOne;
        }
        //Go Game
        loading.allowSceneActivation = true;
    }
}
