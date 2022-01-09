using UnityEngine;
using UnityEngine.UI;

public class UIVsPInfo : MonoBehaviour
{
    public PlayerProperty Property;

    // Start is called before the first frame update
    void Start()
    {
        LoadProperty();
    }

    public void LoadProperty()
    {
        if (GameData.GetVersion() == null)
        {
            return;
        }

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
                    myimage.sprite = ResourcesServices.LoadCharacterIcon(user.Icon);
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
