using TMPro;
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
    CharacterName,
    Description
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

    public void SetText(string text)
    {
        Text mytext = GetComponent<Text>();
        if (mytext != null) { mytext.text = text; }

        TMP_Text mytmp = GetComponent<TMP_Text>();
        if (mytmp != null) { mytmp.text = text; }
    }
    public void LoadProperty()
    {
        //Check if the player data exist
        if (!GlobalGameData.Instance.UserIsInit())
        {
            return;
        }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    User user = GlobalGameData.Instance.GetUserData();
                    SetText(user.NikeName);
                }
                break;
            case PlayerProperty.WalletId:
                {
                    User user = GlobalGameData.Instance.GetUserData();
                    SetText(Utils.GetWalletIDShort(user.WalletId)); 
                }
                break;
            case PlayerProperty.Level:
                {
                    //UserProgress userProgress = GlobalGameData.Instance.GetUserProgress();
                    //SetText($"{Lang.GetText("mn_lvl")} {userProgress.GetLevel()}");
                    
                    SetText($"{Lang.GetText("mn_lvl")} {GlobalGameData.Instance.GetUserData().Level}");
                }
                break;
            case PlayerProperty.Xp:
                {
                    //Cantidad de CXP que aparece
                    UserProgress userProgress = GlobalGameData.Instance.GetUserProgress();
                    SetText($"{userProgress.GetXp()}");
                }
                break;
            case PlayerProperty.XpProgress:
                {
                    UserProgress userProgress = GlobalGameData.Instance.GetUserProgress();
                    SetText($"{userProgress.GetXp()} / {userProgress.GetNextXpGoal()}");
                }
                break;
            case PlayerProperty.Xpbar:
                {
                    UserProgress userProgress = GlobalGameData.Instance.GetUserProgress();
                    Image myimage = GetComponent<Image>();
                    myimage.fillAmount = (float)userProgress.GetXp() / (float)userProgress.GetNextXpGoal();
                }
                break;
            case PlayerProperty.Character:
                {
                    NFTsCharacter nFTsCharacter = GlobalGameData.Instance.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.ValidateSprite(nFTsCharacter.IconSprite);
                }
                break;
            case PlayerProperty.CharacterName:
                {
                    string key = GlobalGameData.Instance.GetUserCharacter().KeyId;
                    SetText(Lang.GetEntityName(key));
                }
                break;
            case PlayerProperty.Avatar:
                {
                    User user = GlobalGameData.Instance.GetUserData();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarUser(user.Avatar);
                }
                break;
            case PlayerProperty.Score:
                {
                    UserProgress progress = GlobalGameData.Instance.GetUserProgress();
                    SetText(progress.GetBattlePoints().ToString());
                }
                break;
            case PlayerProperty.Emblem:
                {
                    NFTsCharacter nFTsCharacter = GlobalGameData.Instance.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadCharacterEmblem(nFTsCharacter.KeyId);
                }
                break;
            case PlayerProperty.Description:
                {
                    string key = GlobalGameData.Instance.GetUserCharacter().KeyId;
                    SetText(Lang.GetEntityDescription(key));
                }
                break;
        }
    }
}
