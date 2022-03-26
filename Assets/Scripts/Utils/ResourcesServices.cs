using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesServices
{
    static Dictionary<string, string> Factions = new Dictionary<string, string>
    {
        {"ALL","Alliance" },
        {"SPI","Spirats" },
        {"NEU","Neutral" },
    };

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

    public static Sprite LoadAvatarIcon(int id)
    {
        return Resources.Load<Sprite>($"UI/Icons/Avatars/Avatar_{id}");
    }

    public static Sprite LoadCharacterIcon(string icon)
    {
        return Resources.Load<Sprite>($"UI/Characters/{icon}");
    }

    public static Sprite LoadCardIcon(string icon, bool isSkill)
    {
        string folder = isSkill ? "Skills" : "Units";
        return Resources.Load<Sprite>($"UI/Icons/{folder}/{icon}");
    }

    public static GameObject LoadCardPrefab(string key, bool isSkill)
    {
        string folder = isSkill ? "Skills" : "Units";
        return Resources.Load<GameObject>($"Prefabs/{folder}/{Factions[key.Substring(2,3)]}/{key}");
    }

    public static GameObject LoadCharacterPrefab(string key)
    {
        return Resources.Load<GameObject>($"Prefabs/Characters/{key}");
    }

    public static Sprite LoadCharacterEmblem(string key)
    {
        return Resources.Load<Sprite>($"UI/Characters/Emblems/{Emblems[key]}");
    }
}
