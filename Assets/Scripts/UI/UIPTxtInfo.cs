using UnityEngine;
using UnityEngine.UI;

public class UIPTxtInfo : MonoBehaviour
{
    public enum PlayerProperty
    {
        Name,
        WalletId,
        Level,
        Xp,
        Xpbar
    }

    public PlayerProperty Property;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameData.UserIsInit())
        {
            return;
        }

        User user = GameData.GetUserData();
        UserProgress userProgress = GameData.GetUserProgress();

        switch(Property)
        {
            case PlayerProperty.Name:
                {
                    Text mytext = GetComponent<Text>();
                    mytext.text = user.NikeName;
                }break;
            case PlayerProperty.WalletId:
                {
                    Text mytext = GetComponent<Text>();
                    mytext.text = Utils.GetWalletIDShort(user.WalletId);
                }
                break;
            case PlayerProperty.Level:
                {
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{Lang.GetText("mn_lvl")} {userProgress.GetLevel()}";
                }
                break;
            case PlayerProperty.Xp:
                {
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{userProgress.GetXp()} {Lang.GetText("mn_xp")}";
                }
                break;
            case PlayerProperty.Xpbar:
                {
                    Image myimage = GetComponent<Image>();
                    myimage.fillAmount = (float)userProgress.GetXp() / (float)userProgress.GetNextXpGoal();
                }break;
        }
    }
}
