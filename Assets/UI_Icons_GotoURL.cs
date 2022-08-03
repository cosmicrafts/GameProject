using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Icons_GotoURL : MonoBehaviour
{
   
    public void GoURLPage(string URL = "https://cosmicrafts.com/")
    {
        if (URL == "" || string.IsNullOrEmpty(URL)) { URL = "https://cosmicrafts.com/"; }
        Application.OpenURL(URL);
    }
    
}
