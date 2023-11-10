using UnityEngine;
using System.Linq;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GameData GMD;

    private void Awake()
    {
        if(GMD != null){Destroy(gameObject);}
        GMD = new GameData();
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy() { GMD = null; }
    
}

public enum Match { bots, multi }
public enum Plataform { Web, Pc }
public enum NFTClass { Character, Skill, Station, Ship }

public class GameData
{
    
    public Match CurrentMatch = Match.multi;
    public Plataform CurrentPlataform = Plataform.Pc;
    public bool DataReady = false;
    Config config;
    User PlayerUser;
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
    public void SetUser(User user)
    {
        PlayerUser = user;
    }
    
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
                Avatar = PlayerPrefs.HasKey("savedAvatar") ? PlayerPrefs.GetInt("savedAvatar") : 1 };
        }
        else{
            
            PlayerUser.Avatar = PlayerPrefs.HasKey("savedAvatar") ? PlayerPrefs.GetInt("savedAvatar") : 1 ;
            
        }

        return PlayerUser;
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
            } else if (PlayerPrefs.HasKey("CharacterSaved"))
            {
                PlayerCharacter = Characters.FirstOrDefault(f=>f.ID == PlayerPrefs.GetInt("CharacterSaved") );
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