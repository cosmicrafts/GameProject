using UnityEngine;

/*
 * This is the game resources services
 * Returns assets from the resources folder
 */

public static class ResourcesServices
{
    //Global Manager Prefab
    public static GameObject LoadGlobalManager()
    {
        return Resources.Load<GameObject>($"Prefabs/Manager/GlobalManagerObj");
    }
    //Returns a sprite avatar
    public static Sprite LoadAvatarIcon(int id)
    {
        return Resources.Load<Sprite>($"UI/Icons/Avatars/Avatar_{id}");
    }
    //Returns a sprite character icon
    public static Sprite LoadCharacterIcon(string nftCharacterKey)
    {
        return Resources.Load<Sprite>($"UI/Characters/Ico_{nftCharacterKey}");
    }
    //Returns a sprite card icon
    public static Sprite LoadCardIcon(string nftCardKey)
    {
        return Resources.Load<Sprite>($"UI/Icons/Cards/Ico_{nftCardKey}");
    }
    //Returns the prefab of a spell or unit
    public static GameObject LoadCardPrefab(string key, bool isSkill)
    {
        string folder = isSkill ? "Skills" : "Units";
        return Resources.Load<GameObject>($"Prefabs/{folder}/{key.Substring(2, 3)}/{key}");
    }
    //Returns the prefab base station from a faction
    public static GameObject LoadBaseStationPrefab(string nftCharacterKey)
    {
        return Resources.Load<GameObject>($"Prefabs/BaseStations/BS_{nftCharacterKey}");
    }
    //Returns the prefab of a character
    public static GameObject LoadCharacterPrefab(string key)
    {
        return Resources.Load<GameObject>($"Prefabs/Characters/{key}");
    }
    //Returns the sprite emblem of a character
    public static Sprite LoadCharacterEmblem(string nftCharacterKey)
    {
        return Resources.Load<Sprite>($"UI/Characters/Emblems/{nftCharacterKey}_Emb");
    }
}
