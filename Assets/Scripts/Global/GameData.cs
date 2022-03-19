using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

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

    public static bool ImMaster = false;

    public static bool DebugMode = false;

    static Config config;

    static User PlayerUser;

    static UserGeneral VsPlayerUser;

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

    public static void SetVsUser(UserGeneral user)
    {
        VsPlayerUser = user;
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
            PlayerUser = new User() {NikeName = "Tester", WalletId = "TestWalletId", Avatar = 1};
        }

        return PlayerUser;
    }

    public static UserGeneral GetVsUser()
    {
        return VsPlayerUser;
    }

    public static UserGeneral BuildMyProfileHasVS()
    {
        if (PlayerUser == null || PlayerProgress == null)
            return null;

        return new UserGeneral()
        {
            WalletId = PlayerUser.WalletId,
            NikeName = PlayerUser.NikeName,
            Level = PlayerProgress.GetLevel(),
            Xp = PlayerProgress.GetXp(),
            Avatar = PlayerUser.Avatar,
            Icon = PlayerCharacter.Icon
        };
    }

    public static UserProgress GetUserProgress()
    {
        if (PlayerProgress == null)
        {
            PlayerProgress = new UserProgress();
            PlayerProgress.InitValues(new Progress { BattlePoints = 0, Xp = 0, Level = 1 });
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
            PlayerCharacter = new NFTsCharacter() { KeyId = "Chr_1", Name = "Sotzeer", Icon = "Character_1" };
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

    public static string GetVersion()
    {
        return Application.version;
    }
}
