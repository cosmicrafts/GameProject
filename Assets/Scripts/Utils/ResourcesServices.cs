using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesServices
{
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
        return Resources.Load<GameObject>($"Prefabs/{folder}/{key}");
    }

    public static GameObject LoadCharacterPrefab(string key)
    {
        return Resources.Load<GameObject>($"Prefabs/Characters/{key}");
    }
}
