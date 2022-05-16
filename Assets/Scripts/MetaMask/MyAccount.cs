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
    [SerializeField]
    InputField newNameInput;
    
    [SerializeField] int avatarIndex;
    public int AvatarIndex => avatarIndex;

    string playerName;
    [SerializeField] Text walletID;
    [SerializeField] Text nickName;
    string walletAccountID;
    public string WalletAccountID => walletAccountID;
    
    
    private void Awake()
    {
        accountPanel.SetActive(false);
        SetAccountName();
        SetAvatar();
        SetWalletID();
       
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
    void SetAccountName()
    {
        if (PlayerPrefs.HasKey("AccounName"))
        {
            playerName = PlayerPrefs.GetString("AccounName");
        }
        if (nickName)
        {
            nickName.text = playerName;
        }

    }
    void SetAvatar()
    {
        if (!PlayerPrefs.HasKey("AvatarIndex"))
        {
            mainAvatarImg.sprite = avatarsIcon[0];

        }
    }
    void SetWalletID()
    {
        if (PlayerPrefs.HasKey("Account"))
        {

            walletAccountID = PlayerPrefs.GetString("Account");

            walletID.text = PlayerPrefs.GetString("Account");
        }


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
    public void ChangeName(string newName) 
    {
        if (PlayerPrefs.HasKey("AccounName"))
        {

        }
    }

}
