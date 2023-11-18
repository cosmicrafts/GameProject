using Newtonsoft.Json;
using UnityEngine;

//Game configuration structure (will be encrypted)
[System.Serializable]
public class Config
{
    public int language = 0;
    public int characterSavedID = 0;
    public int avatarSavedID = 1;

    public Match currentMatch = Match.multi;
    public int modeSelected = 9;
    //public bool AutoLog = true;
    //public SocialLogs TypeLog = SocialLogs.None;    
}

/*
 * This is the local data controller
 * Is used to save information, like the game configuration, on the device */
public static class SaveData 
{
    
    public readonly static string keyConfig = "Config";
    
    //Load the configuration data
    public static void LoadGameConfig()
    {
        Config config = JsonConvert.DeserializeObject<Config>(PlayerPrefs.GetString(keyConfig));
        
        if (config == null)
        {
            //Create a new game config and set it
            Config newConfig = new Config();
            GlobalGameData.Instance.SetConfig(newConfig);
            Lang.InitLanguage((Language)newConfig.language);
        }
        else
        {
            //Set the configuration
            GlobalGameData.Instance.SetConfig(config);
            Lang.InitLanguage((Language)config.language);
        }
        

    }
    //Save the current game configuration locally
    public static void SaveGameConfig(){ PlayerPrefs.SetString( keyConfig, JsonConvert.SerializeObject(GlobalGameData.Instance.GetConfig()) ); }
    
    
    
   
}
