using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarImages : MonoBehaviour
{
    public void SetAvatarsSprites(int newAvatarIndex)
    {
        PlayerPrefs.SetInt("savedAvatar", newAvatarIndex);

        User userData = GlobalManager.GMD.GetUserData();
        userData.Avatar = newAvatarIndex;
        GlobalManager.GMD.SetUser(userData);
        
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.Avatar);
    }
}
