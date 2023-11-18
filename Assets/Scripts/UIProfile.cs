using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{


    public Transform contentAvatars;
    public GameObject avatarPrefab;
    private int avatarNumber = 30;

    private void Start()
    {
        InstanceAvatarsButtons();
    }

    public void InstanceAvatarsButtons()
    {
        for (int i = 1; i <= avatarNumber; i++)
        {
            int avatarIndex = i;
            GameObject avatarButton = Instantiate(avatarPrefab, contentAvatars);
            avatarButton.GetComponent<Image>().sprite = ResourcesServices.LoadAvatarUser(avatarIndex);
            avatarButton.transform.GetComponentInChildren<Button>().onClick.AddListener(() => { SetAvatarsSprites(avatarIndex); });
            avatarButton.SetActive(true);
        }
    }
    
    public void SetAvatarsSprites(int newAvatarIndex)
    {
        GlobalGameData.Instance.SetUserAvatar(newAvatarIndex);
        UIMainMenu.Instance.RefreshProperty(PlayerProperty.Avatar);
    }
}
