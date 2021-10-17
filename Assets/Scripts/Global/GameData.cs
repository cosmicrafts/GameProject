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

    static Config config;

    public static User PlayerUser;

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

    public static User GetUserData()
    {
        if (PlayerUser == null)
        {
            PlayerUser = new User() {NikeName = "Player"};
        }

        return PlayerUser;
    }

    public static void ChangeLang(Language newlang)
    {
        Lang.SetLang(newlang);

        config.language = (int)newlang;
        SaveData.SaveGameConfig();
    }

    [DllImport("__Internal")]
    public static extern void SaveScore(int score);
}
