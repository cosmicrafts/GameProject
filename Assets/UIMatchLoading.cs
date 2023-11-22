using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    //Loading Bar (used when a new scene is loading)
    public Image LocalGameLoadingBar;

    public User MyUser = new User();
    public User VsUser = new User();


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    /*
    Debug.Log("MATCH STARTING");
        
    VsUser vsUser = JsonConvert.DeserializeObject<VsUser>(json);*/
    
    public void GL_MatchStarting(User MyUserData, User VsUserData)
    {
        MyUser = MyUserData;
        VsUser = VsUserData;
        
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
        StartCoroutine(LoadLocalGame());
        
    }
    
    IEnumerator LoadLocalGame()
    {
        yield return new WaitForSeconds(1f);
        
        AsyncOperation loading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        while (!loading.isDone)
        {
            yield return null;
            LocalGameLoadingBar.fillAmount = loading.progress;
        }
    }
    
    public void OnInitMatch()
    {
        Debug.Log("MATCH FINISH");
        
        Destroy(this.gameObject);
    }
    
    
}
