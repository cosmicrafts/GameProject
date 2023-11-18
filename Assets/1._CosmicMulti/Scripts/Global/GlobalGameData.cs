using UnityEngine;
using System.Linq;
using UnityEngine;

public enum Match { bots, multi }
public enum Plataform { Web, Pc }
public enum NFTClass { Character, Skill, Station, Ship }

public class GlobalGameData : MonoBehaviour
{
    private static GlobalGameData _instance;
    public static GlobalGameData Instance {
        get 
        {
            if (_instance == null) { _instance = Instantiate( ResourcesServices.LoadGlobalManager().GetComponent<GlobalGameData>() ); }
            return _instance;
        }
    }
    private void Awake() {  
        if(!_instance){ _instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }
    //private void OnDestroy() { _instance = null; Debug.Log("GMD IS NULL"); }
    
    public Plataform CurrentPlataform = Plataform.Pc;
    public bool DataReady = false;
    Config config;
    User PlayerUser;
    User VsPlayerUser;
    UserProgress PlayerProgress;
    
    UserCollection PlayerCollection;
    NFTsCharacter PlayerCharacter;
    NFTsCollection NFTCollection;
    
    public Config GetConfig()
    {
        if (config == null) { config = new Config(); }
        return config;
    }
    public void SetConfig(Config newconfig)
    {
        config = newconfig;
    }
    public Language GetGameLanguage()
    {
        return (Language)GetConfig().language;
    }
    public void SetUser(User user) { PlayerUser = user; }
    public void SetVsUser(User user) { VsPlayerUser = user; }
    public void SetUserProgress(UserProgress userprogress)
    {
        PlayerProgress = userprogress;
    }
    //Set the NFTs collection data for the player
    public void SetUserCollection(UserCollection userCollection)
    {
        PlayerCollection = userCollection;
    }
    //Set the current player character
    public void SetUserCharacter(int NFTid)
    {
        NFTsCharacter character = GetUserCollection().Characters.FirstOrDefault(f => f.ID == NFTid);
        if (character == null)
        {
            PlayerCharacter = PlayerCollection.Characters[0];
        } else
        {
            PlayerCharacter = character;
        }
        PlayerCollection.ChangeDeckFaction(PlayerCharacter);
        
        config.characterSavedID = NFTid;
        SaveData.SaveGameConfig();
    }
    public void SetUserAvatar(int AvatarSelected)
    {
        config.avatarSavedID = AvatarSelected;
        SaveData.SaveGameConfig();
    }
    public void SetCurrentMatch(Match match)
    {
        config.currentMatch = match;
        SaveData.SaveGameConfig();
    }
    //Set the global NFTs Collection
    public void SetNFTsCollection(NFTsCollection nFTsCollection)
    {
        NFTCollection = nFTsCollection;
    }
    //Returns the basic Player data
    public User GetUserData()
    {
        if (PlayerUser == null)
        {
            
            PlayerUser = new User() {NikeName = "Tester", WalletId = "TestWalletId", 
                Avatar = config.avatarSavedID };
        }
        else{
            
            PlayerUser.Avatar = config.avatarSavedID ;
            
        }

        return PlayerUser;
    }
    public User GetVsUserData()
    {
        if (VsPlayerUser == null)
        {
            
            VsPlayerUser = new User() {NikeName = "Tester", WalletId = "TestWalletId", 
                Avatar = PlayerPrefs.HasKey("savedAvatar") ? PlayerPrefs.GetInt("savedAvatar") : 1 };
        }
        else{
            
            VsPlayerUser.Avatar = PlayerPrefs.HasKey("savedAvatar") ? PlayerPrefs.GetInt("savedAvatar") : 1 ;
            
        }

        return VsPlayerUser;
    }

    public UserProgress GetUserProgress()
    {
        if (PlayerProgress == null)
        {
            PlayerProgress = new UserProgress();
            PlayerProgress.InitValues(new Progress { BattlePoints = 0, Xp = 0, Level = 1 });
        }

        return PlayerProgress;
    }
    //Returns the NFTs data of the player
    public UserCollection GetUserCollection()
    {
        if (PlayerCollection == null)
        {
            PlayerCollection = new UserCollection();
            PlayerCollection.InitCollection();
        }

        return PlayerCollection;
    }
    //Returns the current player character
    public NFTsCharacter GetUserCharacter()
    {
        if (PlayerCharacter == null)
        {
            var Characters = GetUserCollection().Characters;
            if (Characters == null)
            {
                PlayerCharacter = GetUserCollection().DefaultCharacter;
            } else if (Characters.Count == 0)
            {
                PlayerCharacter = GetUserCollection().DefaultCharacter;
            } else if ( GetConfig().characterSavedID != 0)
            {
                PlayerCharacter = Characters.FirstOrDefault(f=>f.ID == GetConfig().characterSavedID );
            }
            else
            {
                PlayerCharacter = Characters[0];
            }
        }

        return PlayerCharacter;
    }
    //Returns the Global NFTs collection
    public NFTsCollection GetNFTsCollection()
    {
        if (NFTCollection == null)
        {
            NFTCollection = new NFTsCollection();
            NFTCollection.InitGlobalCollection();
        }

        return NFTCollection;
    }
    //Change the language of the game
    public void ChangeLang(Language newlang)
    {
        Lang.SetLang(newlang);

        config.language = (int)newlang;
        SaveData.SaveGameConfig();
    }
    
    
    //Returns if the player information is loaded and ready
    public bool UserIsInit()
    {
        return PlayerUser != null;
    }
    //Clear the player information
    public void ClearUser()
    {
        PlayerUser = null;
        PlayerProgress = null;
        PlayerCharacter = null;
        PlayerCollection = null;
    }
    public string GetVersion() { return Application.version; }
    
    
    
    
    
    
    
}

