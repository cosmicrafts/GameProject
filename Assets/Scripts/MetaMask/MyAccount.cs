using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyAccount : MonoBehaviour
{
    [SerializeField]
    GameObject backButton;
    [SerializeField]
    GameObject accountPanel;
    [SerializeField]
    GameObject avatarsHolder;
    [SerializeField]
    List<Sprite> avatarsIcon =new List<Sprite>();
    [SerializeField]
    int accountLevel;
    [SerializeField]
    int accountExp;
    [SerializeField]
    Image mainAvatarImg;
    [SerializeField]
    List<int> expToLvlUP;

    [SerializeField] int avatarIndex;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("AvatarIndex"))
        {
            avatarIndex = PlayerPrefs.GetInt("AvatarIndex");
            mainAvatarImg.sprite = avatarsIcon[avatarIndex];

        }
        accountPanel.SetActive(false);
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("AvatarIndex"))
        {
            mainAvatarImg.sprite = avatarsIcon[0];

        }
    
       
    }
    public void OnActiveAccountPanel()
    {
        
        accountPanel.SetActive(true);
      
    }
    public void OnClosedAccountPanel()
    {

        accountPanel.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {

        AccountLevelUp();
    }
    void AccountLevelUp()
    {
       
    }
    public void ChangeAvatarSprite(int newAvatar)
    {
        avatarIndex = newAvatar;
        mainAvatarImg.sprite = avatarsIcon[avatarIndex];
        PlayerPrefs.SetInt("AvatarIndex", newAvatar);
    }
  
    public void AddAccountExperencie(int newExp)
    {
        accountExp += newExp;
        if (accountLevel >= expToLvlUP.Count)
        {
            return;
        }

        if (accountExp  >= expToLvlUP[accountLevel])
        {
            accountLevel++;
        }
    }
    public void ChangeName() 
    {
    
    }

}
