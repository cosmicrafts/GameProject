using UnityEngine;
using UnityEngine.UI;

//Types of properties of the player
public enum PlayerProperty
{
    Name,
    WalletId,
    Level,
    Xp,
    Xpbar,
    Character,
    Avatar,
    Score,
    XpProgress,
    Emblem
}

public class UIPTxtInfo : MonoBehaviour
{
    //The player property
    public PlayerProperty Property;

    //Load and show the property when begins
    void Start()
    {
        LoadProperty();
    }

    public void LoadProperty()
    {
        //Check if the player data exist
        if (!GameData.UserIsInit())
        {
            return;
        }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    User user = GameData.GetUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = user.NikeName;
                }
                break;
            case PlayerProperty.WalletId:
                {
                    User user = GameData.GetUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = Utils.GetWalletIDShort(user.WalletId);
                }
                break;
            case PlayerProperty.Level:
                {
                    UserProgress userProgress = GameData.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{Lang.GetText("mn_lvl")} {userProgress.GetLevel()}";
                }
                break;
            case PlayerProperty.Xp:
                {
                    UserProgress userProgress = GameData.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{userProgress.GetXp()} {Lang.GetText("mn_xp")}";
                }
                break;
            case PlayerProperty.XpProgress:
                {
                    UserProgress userProgress = GameData.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{userProgress.GetXp()} / {userProgress.GetNextXpGoal()} {Lang.GetText("mn_xp")}";
                }
                break;
            case PlayerProperty.Xpbar:
                {
                    UserProgress userProgress = GameData.GetUserProgress();
                    Image myimage = GetComponent<Image>();
                    myimage.fillAmount = (float)userProgress.GetXp() / (float)userProgress.GetNextXpGoal();
                }
                break;
            case PlayerProperty.Character:
                {
                    NFTsCharacter nFTsCharacter = GameData.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadCharacterIcon(nFTsCharacter.Icon);
                }
                break;
            case PlayerProperty.Avatar:
                {
                    User user = GameData.GetUserData();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
                }
                break;
            case PlayerProperty.Score:
                {
                    UserProgress progress = GameData.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = progress.GetBattlePoints().ToString();
                }
                break;
            case PlayerProperty.Emblem:
                {
                    NFTsCharacter nFTsCharacter = GameData.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadCharacterEmblem(nFTsCharacter.KeyId);
                }
                break;
        }
    }
}
