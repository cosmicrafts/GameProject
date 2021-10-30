using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public string user { get; set; }
    public string password { get; set; }

    public Text label_status;

    public GameObject TestEndpoints;

    public GameObject btn_login;

    public GameObject social;

    //public PH_Connect PHCtrl;

    private void Start()
    {
        SaveData.LoadGameConfig();
    }

    public async void RequestLogin()
    {
        SetStatusText("Conectando...", Color.blue);

        NetResult netr = await NetUserServices.LogIn(user, password); 

        if (netr.Status == EStatus.success)
        {
            SuccessLogin(netr.Response);
        } else
        {
            SetStatusText(netr.Response, Color.red);
        }
    }

    public void SuccessLogin(string jsonUser)
    {
        SetStatusText(string.Empty);
        GameData.SetUser(JsonConvert.DeserializeObject<User>(jsonUser));
        //Photon.Pun.PhotonNetwork.NickName = GameData.PlayerUser.NikeName;
        //PHCtrl.ConnectPhotonServer();
        TestEndpoints.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetStatusText(string message, Color color)
    {
        label_status.text = message;
        label_status.color = color;
    }

    public void SetStatusText(string message)
    {
        label_status.text = message;
        label_status.color = Color.black;
    }

    public void SetActiveLoginBtns(bool active)
    {
        btn_login.SetActive(active);
        social.SetActive(active);
    }
}
