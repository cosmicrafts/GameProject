using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerGameInfo : MonoBehaviour
{
    public Text PlayerName;
    public Text WalletId;
    public Text Level;
    public Image XpBar;
    public Image Avatar;
    public Image Icon;

    public void InitInfo(User user, UserProgress progress, NFTsCharacter character)
    {
        WalletId.text = Utils.GetWalletIDShort(user.WalletId);
        PlayerName.text = user.NikeName;
        Level.text = $"{Lang.GetText("mn_lvl")} {progress.GetLevel()}";
        XpBar.fillAmount = (float)progress.GetXp() / (float)progress.GetNextXpGoal();
        Avatar.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
        Icon.sprite = ResourcesServices.LoadCharacterIcon(character.Icon);
    }

    public void InitInfo(UserGeneral user)
    {
        WalletId.text = Utils.GetWalletIDShort(user.WalletId);
        PlayerName.text = user.NikeName;
        Level.text = $"{Lang.GetText("mn_lvl")} {user.Level}";
        XpBar.fillAmount = (float)user.Xp / (float)user.GetNextXpGoal();
        Avatar.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
        Icon.sprite = ResourcesServices.LoadCharacterIcon(user.Icon);
    }
}
