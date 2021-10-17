using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPTxtInfo : MonoBehaviour
{
    public enum PlayerProperty
    {
        Name,
        WalletId
    }

    public PlayerProperty Property;

    // Start is called before the first frame update
    void Start()
    {
        if (GameData.PlayerUser == null)
        {
            return;
        }

        Text mytext = GetComponent<Text>();
        switch(Property)
        {
            case PlayerProperty.Name:
                {
                    mytext.text = GameData.PlayerUser.NikeName;
                }break;
            case PlayerProperty.WalletId:
                {
                    mytext.text = Utils.GetWalletIDShort(GameData.PlayerUser.WalletId);
                }
                break;
        }
    }
}
