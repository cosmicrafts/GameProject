using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchLoading : MonoBehaviour
{
    [Header("UI Match and Stats from Game")]
    public GameObject MatchLoadingScreen;
    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;
    public Text Txt_VsLevel;
    public Image Img_VsIcon;
    public Image Img_VsEmblem;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    /*
    Debug.Log("MATCH STARTING");
        
    VsUser vsUser = JsonConvert.DeserializeObject<VsUser>(json);*/
    
    public void GL_MatchStarting(User MyUserData, User VsUserData)
    {
        Debug.Log("MATCH STARTING");
        
        Txt_VsWalletId.text = VsUserData.WalletId;
        Txt_VsNikeName.text = VsUserData.NikeName;
        Txt_VsLevel.text = VsUserData.Level.ToString();
        
        var Characters = GlobalGameData.Instance.GetUserCollection().Characters;
        NFTsCharacter vsCharacter = Characters.FirstOrDefault(f=>f.ID == VsUserData.CharacterNFTId );
        if (vsCharacter != null)
        {
            Img_VsIcon.sprite = vsCharacter.IconSprite;
            Img_VsEmblem.sprite = ResourcesServices.LoadCharacterEmblem(vsCharacter.KeyId);
        }
        
       // SearchingScreen.SetActive(false);
        //AcceptMatchScreen.SetActive(false);
        MatchLoadingScreen.SetActive(true);
    }
    
    public void OnInitMatch()
    {
        Debug.Log("MATCH FINISH");
        
        Destroy(this.gameObject);
    }
    
    
}
