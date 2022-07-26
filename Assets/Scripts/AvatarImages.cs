using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarImages : MonoBehaviour
{
    [SerializeField]
    List<Sprite> imagesForAvatars;
    public List<Sprite> ImagesForAvatars => imagesForAvatars;
    int avatarInUsed;
    [SerializeField]
    Image avatar;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("AvatarMain"))
        {
            avatarInUsed = PlayerPrefs.GetInt("AvatarMain");
            avatar.sprite = imagesForAvatars[avatarInUsed];
        }
        else
        {

            avatarInUsed = 0;
            avatar.sprite = imagesForAvatars[avatarInUsed];

            PlayerPrefs.SetInt("AvatarMain", 0);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetAvatarsSprites(int newAvatarIndex)
    {
        avatarInUsed = newAvatarIndex;
        avatar.sprite = imagesForAvatars[newAvatarIndex];
        // lobbyUI.SetOthersAvatars();
        PlayerPrefs.SetInt("AvatarMain", avatarInUsed);
    }
}
