using Newtonsoft.Json;
using UnityEngine;

public static class SaveData 
{
    //The path of the game configuration file
    public readonly static string fullPathConfig = Application.persistentDataPath + "/" + "Config";

    public static bool error = false;
    //Load the configuration data
    public static void LoadGameConfig()
    {
        SaveManager.Instance.Load<Config>(fullPathConfig, DataWasLoaded, false);
    }
    //This function is called when the configuration was loaded
    private static void DataWasLoaded(Config data, SaveResult result, string message)
    {
        //Check for some error or emptu data
        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            //Create a new game config and set it
            Config newConfig = new Config();
            GameData.SetConfig(newConfig);
            //initialize the language
            Lang.InitLanguage((Language)newConfig.language);
            return;
        }
        //Success load
        if (result == SaveResult.Success)
        {
            //Set the configuration
            GameData.SetConfig(data);
            //initialize the language
            Lang.InitLanguage((Language)data.language);
        }
    }
    //Save the current game configuration locally
    public static void SaveGameConfig()
    {
        SaveManager.Instance.Save(GameData.GetConfig(), fullPathConfig, DataWasSaved, false);
        if (GameData.IsProductionWeb())
        {
            GameNetwork.JSSavePlayerConfig(JsonConvert.SerializeObject(GameData.GetConfig()));
        }
    }
    //This function is called when the configuration was saved
    private static void DataWasSaved(SaveResult result, string message)
    {
        error = result == SaveResult.Error;
    }
}
