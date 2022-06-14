using System.Collections.Generic;
using UnityEngine;

/*
 * This is the game resources services
 * Returns assets from the resources folder
 */

public static class ResourcesServices
{
    //Dictionary of factions (prefix)
    static Dictionary<string, string> Factions = new Dictionary<string, string>
    {
        {"ALL","Alliance" },
        {"SPI","Spirats" },
        {"NEU","Neutral" },
    };
    //Dictionary of character´s emblems
    static Dictionary<string, string> Emblems = new Dictionary<string, string>
    {
        {"Chr_0","Emblem_0" },
        {"Chr_1","Emblem_1" },
        {"Chr_2","Emblem_2" },
        {"Chr_3","Emblem_3" },
        {"Chr_4","Emblem_4" },
        {"Chr_5","Emblem_5" },
        {"Chr_6","Emblem_6" },
    };

    //Returns a sprite avatar
    public static Sprite LoadAvatarIcon(int id)
    {
        return Resources.Load<Sprite>($"UI/Icons/Avatars/Avatar_{id}");
    }
    //Returns a sprite character icon
    public static Sprite LoadCharacterIcon(string icon)
    {
        return Resources.Load<Sprite>($"UI/Characters/{icon}");
    }
    //Returns a sprite card icon
    public static Sprite LoadCardIcon(string icon, bool isSkill)
    {
        string folder = isSkill ? "Skills" : "Units";
        return Resources.Load<Sprite>($"UI/Icons/{folder}/{icon}");
    }
    //Returns the prefab of a spell or unit
    public static GameObject LoadCardPrefab(string key, bool isSkill)
    {
        string folder = isSkill ? "Skills" : "Units";
        return Resources.Load<GameObject>($"Prefabs/{folder}/{Factions[key.Substring(2,3)]}/{key}");
    }
    //Returns the prefab base station from a faction
    public static GameObject LoadBaseStationPrefab(string faction)
    {
        return Resources.Load<GameObject>($"Prefabs/BaseStations/BS_{faction}");
    }
    //Returns the prefab of a character
    public static GameObject LoadCharacterPrefab(string key)
    {
        return Resources.Load<GameObject>($"Prefabs/Characters/{key}");
    }
    //Returns the sprite emblem of a character
    public static Sprite LoadCharacterEmblem(string key)
    {
        return Resources.Load<Sprite>($"UI/Characters/Emblems/{Emblems[key]}");
    }
}
