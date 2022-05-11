using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SetPlayerNickName : MonoBehaviour
{
    [SerializeField]
    InputField inputNameField;
    [SerializeField]
    string
    mainScene;
    public User PlayerUser;
    public UserProgress PlayerProgress;
    public UserCollection PlayerCollection;
    public NFTsCharacter PlayerCharacter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitPlayerData()
    {
        PlayerUser = GameData.GetUserData();
        PlayerProgress = GameData.GetUserProgress();
        PlayerCharacter = GameData.GetUserCharacter();
      
      
    }

    public void OnMetaNameData(int usser)
    {
        if (usser ==1)
        {
            SceneManager.LoadScene(mainScene);
            //tutorial
        }
        else 
       
        {
            //Nombre no valido
        }

      

        

    }
    public void SetPlayerName()
    { 
        if(inputNameField.text != null)
        {
            string playerName = inputNameField.text;
            PlayerPrefs.SetString("AccounName", playerName);
     
            GameNetwork.JSMetaUsserName(playerName);
        }


    }
}
