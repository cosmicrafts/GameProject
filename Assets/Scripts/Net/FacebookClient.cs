using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookClient : MonoBehaviour
{
    public Login login;
    // Start is called before the first frame update
    void Start()
    {
        InitFacebook();
    }

    void InitFacebook()
    {

#if UNITY_EDITOR
        return;
#endif

        //if (!FB.IsInitialized)
        //{
        //    FB.Init(InitCallback);
        //}
        //else
        //{
        //    FB.ActivateApp();
        //}

        //if (FB.IsLoggedIn)
        //{
        //    //No Logged
        //}
        //else
        //{
        //    //No Logged
        //}
    }

    void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Error init
            return;
        }

        if (FB.IsLoggedIn)
        {
            //Is Logged
        }
    }

    void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            LoadFacebookDataUser();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void LoadFacebookDataUser()
    {
        FB.API("/me?fields=id,name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
        FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, FbGetPicture);
    }

    private void FbGetPicture(IGraphResult result)
    {
        Sprite UserFBIcon = null;

        if (result.Texture != null)
            UserFBIcon = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
    }

    private void FetchProfileCallback(IGraphResult result)
    {
        Dictionary<string, object> FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;

        Debug.Log(FBUserDetails);

        string UserName = FBUserDetails["name"].ToString();
        string Password = Utils.GeneratePasswordFromID(FBUserDetails["id"].ToString());
        string Email = FBUserDetails["email"].ToString();

        RequestLogin(UserName, Password, Email);
    }

    public void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    public void FacebookLogOUT()
    {
        FB.LogOut();
    }

    public static void FacebookShare()
    {
        FB.ShareLink(new Uri("http://...PromoLink"), "Check it out!", "I love this game", new System.Uri("http://...PromoLogoLink"));
    }

    public static void FacebookGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Cosmicrafts");
    }

    public static void FacebookInvite()
    {
        FB.Mobile.AppInvite(new Uri("http://...StorePage"));
    }

    public static void GetFriendsPlaying()
    {
        List<string> FB_Friends = new List<string>();
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendslist = (List<object>)dictionary["data"];
            foreach (var dict in friendslist)
            {
                string _friend = (string)((Dictionary<string, object>)dict)["name"];
                FB_Friends.Add(_friend);
            }
        });
    }

    async void RequestLogin(string _user, string _pass, string _email)
    {
        login.SetStatusText("Conectando...", Color.black);

        NetResult netr = await NetUserServices.LogInOrAdd(_user, _pass, _email);

        if (netr.Status == EStatus.success)
        {
            login.SuccessLogin(netr.Response);
        }
        else
        {
            login.SetStatusText(netr.Response, Color.red);
        }
    }

}
