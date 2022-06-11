using System.Linq;
using UnityEngine;
/*
    Here we save and manage the global data of the game like
    player's data, game configuration, game language, current game mode, etc.
    This data always exist
 */

//Enum Game Modes Types
public enum Match
{
    tutorial,
    bots,
    multi
}
//Enum Plataforms Types
public enum Plataform
{
    Web,
    Pc
}
public static class GameData
{
    //Save the selected game mode
    public static Match CurrentMatch = Match.multi;
    //Save if the player is the multiplayer master
    public static bool ImMaster = false;
    //Save if the game is running in Debug mode
    public static bool DebugMode = false;
    //Save the current plataform
    public static Plataform CurrentPlataform = Plataform.Pc;
    //Save if the player data is loaded and ready
    public static bool DataReady = false;
    //Save the game configuration
    static Config config;
    //Save the basic data of the player
    static User PlayerUser;
    //Save the basic data of Vs player (for multiplayer)
    static UserGeneral VsPlayerUser;
    //Save the data progression of the player
    static UserProgress PlayerProgress;
    //Save the NFTs collection data of the player
    static UserCollection PlayerCollection;
    //Save the current character selected by the player
    static NFTsCharacter PlayerCharacter;
    //Save te current region of the game
    public static string Region = "LAN";

    //Returns the current game config, if is null, create a new one
    public static Config GetConfig()
    {
        if (config == null)
        {
            config = new Config();
        }

        return config;
    }
    //Set the current game config
    public static void SetConfig(Config newconfig)
    {
        config = newconfig;
    }
    //Get the current game language
    public static Language GetGameLanguage()
    {
        return (Language)GetConfig().language;
    }
    //Set the basic player data
    public static void SetUser(User user)
    {
        PlayerUser = user;
    }
    //Set the basic Vs player data
    public static void SetVsUser(UserGeneral user)
    {
        VsPlayerUser = user;
    }
    //Set the player's progression
    public static void SetUserProgress(UserProgress userprogress)
    {
        PlayerProgress = userprogress;
    }
    //Set the NFTs collection data for the player
    public static void SetUserCollection(UserCollection userCollection)
    {
        PlayerCollection = userCollection;
    }
    //Set the current player character
    public static void SetUserCharacter(string keyId)
    {
        NFTsCharacter character = GetUserCollection().Characters.FirstOrDefault(f => f.KeyId == keyId);
        if (character == null)
        {
            PlayerCharacter = PlayerCollection.Characters[0];
        } else
        {
            PlayerCharacter = character;
        }
        PlayerCollection.ChangeDeckFaction(PlayerCharacter);
    }
    //Returns the basic Player data
    public static User GetUserData()
    {
        if (PlayerUser == null)
        {
            PlayerUser = new User() {NikeName = "Tester", WalletId = "TestWalletId", Avatar = 1};
        }

        return PlayerUser;
    }
    //Returns the basic Vs Player data
    public static UserGeneral GetVsUser()
    {
        return VsPlayerUser;
    }
    //Create a data resume of the player, used to send it to the network
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
            CharacterKey = PlayerCharacter.KeyId
        };
    }
    //Returns the progression of the player
    public static UserProgress GetUserProgress()
    {
        if (PlayerProgress == null)
        {
            PlayerProgress = new UserProgress();
            PlayerProgress.InitValues(new Progress { BattlePoints = 0, Xp = 0, Level = 1 });
        }

        return PlayerProgress;
    }
    //Returns the NFTs data of the player
    public static UserCollection GetUserCollection()
    {
        if (PlayerCollection == null)
        {
            PlayerCollection = new UserCollection();
            PlayerCollection.InitCollection();
        }

        return PlayerCollection;
    }
    //Returns the current player character
    public static NFTsCharacter GetUserCharacter()
    {
        if (PlayerCharacter == null)
        {
            PlayerCharacter = PlayerCollection.Characters[0];
        }

        return PlayerCharacter;
    }
    //Change the language of the game
    public static void ChangeLang(Language newlang)
    {
        Lang.SetLang(newlang);

        config.language = (int)newlang;
        SaveData.SaveGameConfig();
    }
    //Returns if the player information is loaded and ready
    public static bool UserIsInit()
    {
        return PlayerUser != null;
    }
    //Clear the player information
    public static void ClearUser()
    {
        PlayerUser = null;
        PlayerProgress = null;
        PlayerCharacter = null;
        PlayerCollection = null;
        ImMaster = false;
    }
    //Returns the current version of the game
    public static string GetVersion()
    {
        return Application.version;
    }
    //Returns if the game is running on web and if is a production build
    public static bool IsProductionWeb()
    {
        return CurrentPlataform == Plataform.Web && !DebugMode;
    }
}
