using UnityEngine;
using UnityEngine.UI;

/*
 * This code shows a enemy player property on a UI element (UI component)
 * Is only used for multiplayer
 */

public class UIVsPInfo : MonoBehaviour
{
    //The enemy player property
    public PlayerProperty Property;

    //Load and show the property when begins
    void Start()
    {
        LoadProperty();
    }

    public void LoadProperty()
    {
        //Check if we have an enemy data
        if (GlobalGameData.Instance.GetVersion() == null)
        {
            return;
        }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    User user = GlobalGameData.Instance.GetVsUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = user.NikeName;
                }
                break;
            case PlayerProperty.WalletId:
                {
                    User user = GlobalGameData.Instance.GetVsUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = Utils.GetWalletIDShort(user.WalletId);
                }
                break;
            case PlayerProperty.Level:
                {
                    User user = GlobalGameData.Instance.GetVsUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{Lang.GetText("mn_lvl")} {user.Level}";
                }
                break;
            case PlayerProperty.Xp:
                {
                    User user = GlobalGameData.Instance.GetVsUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{user.Level} {Lang.GetText("mn_xp")}";
                }
                break;
            case PlayerProperty.Character:
                {
                    NFTsCharacter character = GlobalGameData.Instance.GetUserCharacter();
                    if (character != null)
                    {
                        Image myimage = GetComponent<Image>();
                        myimage.sprite = ResourcesServices.LoadCharacterIcon(character.KeyId);
                    }
                }
                break;
            case PlayerProperty.Avatar:
                {
                    User user = GlobalGameData.Instance.GetVsUserData();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
                }
                break;
        }
    }
}
