using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Icons_GotoURL : MonoBehaviour
{
   
    public void GoURLPage(string URL = "https://discord.com/invite/cosmicrafts")
    {
        if (URL == "" || string.IsNullOrEmpty(URL)) { URL = "https://discord.com/invite/cosmicrafts"; }
        Application.OpenURL(URL);
    }
    
}
