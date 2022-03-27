using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    private NFTsCharacter Data;

    public Image MyAvatar;
    public Text MyName;
    public GameObject IconSelected;

    public void SetData(NFTsCharacter data)
    {
        Data = data;
        MyAvatar.sprite = ResourcesServices.LoadAvatarIcon(Data.CharacterId);
        MyName.text = Lang.GetEntityName(Data.KeyId);
    }

    public NFTsCharacter GetData()
    {
        return Data;
    }

    public void SetSelection(bool selected)
    {
        IconSelected.SetActive(selected);
    }
}
