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
        if (GameData.GetVersion() == null)
        {
            return;
        }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    UserGeneral user = GameData.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = user.NikeName;
                }
                break;
            case PlayerProperty.WalletId:
                {
                    UserGeneral user = GameData.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = Utils.GetWalletIDShort(user.WalletId);
                }
                break;
            case PlayerProperty.Level:
                {
                    UserGeneral user = GameData.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{Lang.GetText("mn_lvl")} {user.Level}";
                }
                break;
            case PlayerProperty.Xp:
                {
                    UserGeneral user = GameData.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{user.Xp} {Lang.GetText("mn_xp")}";
                }
                break;
            case PlayerProperty.Character:
                {
                    UserGeneral user = GameData.GetVsUser();
                    Image myimage = GetComponent<Image>();
                    NFTsCharacter nFTsCharacter = GameMng.PlayerCollection.GetCharacterByKey(user.CharacterKey);
                    myimage.sprite = ResourcesServices.LoadCharacterIcon(nFTsCharacter.Icon);
                }
                break;
            case PlayerProperty.Avatar:
                {
                    UserGeneral user = GameData.GetVsUser();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
                }
                break;
        }
    }
}
