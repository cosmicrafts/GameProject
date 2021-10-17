using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{
    readonly string ClientId = "8mm6g4mcvt7xcqhdvyxxwsyckh9uki";
    readonly string Redirect = "http://localhost:5000/api/user/twitch";
    private readonly string TwitchBaseUrl = "https://id.twitch.tv/oauth2/";

    public GameObject LoginForm;
    public GameObject TwitchForm;
    public GameObject LoadingTitle;

    public string TwitchEmail { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        LoginForm.SetActive(false);
        TwitchForm.SetActive(true);
    }

    public void Back()
    {
        LoginForm.SetActive(true);
        TwitchForm.SetActive(false);
    }

    public void TokenLogin()
    {
        string claims = "{'id_token':{'email':null,'email_verified':null}}";
        Application.OpenURL($"{TwitchBaseUrl}authorize?client_id={ClientId}&redirect_uri={Redirect}&response_type=code&scope=user:read:email+openid&claims={claims}");

        TwitchForm.SetActive(false);
        LoadingTitle.SetActive(true);

        ResponseLisener();
    }

    async void ResponseLisener()
    {
        int atp = 60;
        while(atp > 0)
        {
            NetResult netr = await NetUserServices.GetTwitchResponse(TwitchEmail);

            if (netr.Status == EStatus.success)
            {
                User user = JsonConvert.DeserializeObject<User>(netr.Response);

                string Password = Utils.GeneratePasswordFromID(user.SocialId);

                Config config = GameData.GetConfig();

                config.Password = Password;
                config.UserName = user.NikeName;
                config.AutoLog = true;
                config.TypeLog = SocialLogs.Twitch;
                
                SaveData.SaveGameConfig();

                //Normal Login

                break;
            }

            await Task.Delay(1000);

            atp--;
        }
    }
}
