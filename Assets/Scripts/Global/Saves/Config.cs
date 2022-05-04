using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SocialLogs
{
    None,
    Facebook,
    Twitch
}

[System.Serializable]
public class Config
{
    public int language = 0; // 0 ingles / 1 español

    public string UserName = string.Empty;

    public string Password = string.Empty;

    public bool AutoLog = true;

    public SocialLogs TypeLog = SocialLogs.None;    
}
