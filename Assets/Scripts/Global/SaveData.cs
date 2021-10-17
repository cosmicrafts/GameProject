using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData 
{
    //the filename where saved data will be stored
    public readonly static string fullPathConfig = Application.persistentDataPath + "/" + "Config";

    public static bool error = false;

    public static void LoadGameConfig()
    {
        SaveManager.Instance.Load<Config>(fullPathConfig, DataWasLoaded, false);
    }

    private static void DataWasLoaded(Config data, SaveResult result, string message)
    {
        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            Config newConfig = new Config();
            GameData.SetConfig(newConfig);
            Lang.InitLanguage((Language)newConfig.language);
            return;
        }

        if (result == SaveResult.Success)
        {
            GameData.SetConfig(data);
            Lang.InitLanguage((Language)data.language);
        }
    }

    public static void SaveGameConfig()
    {
        SaveManager.Instance.Save(GameData.GetConfig(), fullPathConfig, DataWasSaved, false);
    }

    private static void DataWasSaved(SaveResult result, string message)
    {
        error = result == SaveResult.Error;
    }
}
