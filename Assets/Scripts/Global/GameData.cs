using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public enum Match
{
    none,
    tutorial,
    bots,
    multi
}
public static class GameData
{
    public static Match CurrentMatch = Match.none;

    public static bool DebugMode = false;

    public static bool DataIsInit = false;

    static Config config;

    static User PlayerUser;

    static UserProgress PlayerProgress;

    static UserCollection PlayerCollection;

    static NFTsCharacter PlayerCharacter;

    public static string Region = "LAN";

    public static Config GetConfig()
    {
        if (config == null)
        {
            config = new Config();
        }

        return config;
    }

    public static void SetConfig(Config newconfig)
    {
        config = newconfig;
    }

    public static void SetUser(User user)
    {
        PlayerUser = user;
    }

    public static void SetUserProgress(UserProgress userprogress)
    {
        PlayerProgress = userprogress;
    }

    public static void SetUserCollection(UserCollection userCollection)
    {
        PlayerCollection = userCollection;
    }

    public static void SetUserCharacter(NFTsCharacter nFTsCharacter)
    {
        PlayerCharacter = nFTsCharacter;
    }

    public static User GetUserData()
    {
        if (PlayerUser == null)
        {
            PlayerUser = new User() {NikeName = "Player", Avatar = 1};
        }

        return PlayerUser;
    }

    public static UserProgress GetUserProgress()
    {
        if (PlayerProgress == null)
        {
            PlayerProgress = new UserProgress();
            PlayerProgress.InitValues(1, 0);
        }

        return PlayerProgress;
    }

    public static UserCollection GetUserCollection()
    {
        if (PlayerCollection == null)
        {
            PlayerCollection = new UserCollection();
            PlayerCollection.InitCollection();
        }

        return PlayerCollection;
    }

    public static NFTsCharacter GetUserCharacter()
    {
        if (PlayerCharacter == null)
        {
            PlayerCharacter = new NFTsCharacter() { Name = "Sotzeer", Icon = "Character_1" };
        }

        return PlayerCharacter;
    }

    public static void ChangeLang(Language newlang)
    {
        Lang.SetLang(newlang);

        config.language = (int)newlang;
        SaveData.SaveGameConfig();
    }

    public static bool UserIsInit()
    {
        return PlayerUser != null;
    }

    public static void ClearUser()
    {
        PlayerUser = null;
        PlayerProgress = null;
    }

    [DllImport("__Internal")]
    public static extern void SaveScore(int score);
}
