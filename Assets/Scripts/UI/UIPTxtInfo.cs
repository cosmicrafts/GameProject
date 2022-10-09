using UnityEngine;
using UnityEngine.UI;

/*
 * This code shows a player property on a UI element (UI component)
 */

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
    Emblem,
    CharacterName
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
        if (!GlobalManager.GMD.UserIsInit())
        {
            return;
        }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    User user = GlobalManager.GMD.GetUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = user.NikeName;
                }
                break;
            case PlayerProperty.WalletId:
                {
                    User user = GlobalManager.GMD.GetUserData();
                    Text mytext = GetComponent<Text>();
                    mytext.text = Utils.GetWalletIDShort(user.WalletId);
                }
                break;
            case PlayerProperty.Level:
                {
                    UserProgress userProgress = GlobalManager.GMD.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{Lang.GetText("mn_lvl")} {userProgress.GetLevel()}";
                }
                break;
            case PlayerProperty.Xp:
                {
                    //Cantidad de CXP que aparece
                    UserProgress userProgress = GlobalManager.GMD.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{userProgress.GetXp()}"; //{Lang.GetText("mn_xp")} add for multilang
                }
                break;
            case PlayerProperty.XpProgress:
                {
                    UserProgress userProgress = GlobalManager.GMD.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{userProgress.GetXp()} / {userProgress.GetNextXpGoal()} ";//{Lang.GetText("mn_xp")}
                }
                break;
            case PlayerProperty.Xpbar:
                {
                    UserProgress userProgress = GlobalManager.GMD.GetUserProgress();
                    Image myimage = GetComponent<Image>();
                    myimage.fillAmount = (float)userProgress.GetXp() / (float)userProgress.GetNextXpGoal();
                }
                break;
            case PlayerProperty.Character:
                {
                    NFTsCharacter nFTsCharacter = GlobalManager.GMD.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.ValidateSprite(nFTsCharacter.IconSprite);
                }
                break;
            case PlayerProperty.CharacterName:
                {
                    NFTsCharacter nFTsCharacter = GlobalManager.GMD.GetUserCharacter();
                    Text mytext = GetComponent<Text>();
                    mytext.text = nFTsCharacter.Name;
                }
                break;
            case PlayerProperty.Avatar:
                {
                    User user = GlobalManager.GMD.GetUserData();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
                }
                break;
            case PlayerProperty.Score:
                {
                    UserProgress progress = GlobalManager.GMD.GetUserProgress();
                    Text mytext = GetComponent<Text>();
                    mytext.text = progress.GetBattlePoints().ToString();
                }
                break;
            case PlayerProperty.Emblem:
                {
                    NFTsCharacter nFTsCharacter = GlobalManager.GMD.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadCharacterEmblem(nFTsCharacter.KeyId);
                }
                break;
        }
    }
}
