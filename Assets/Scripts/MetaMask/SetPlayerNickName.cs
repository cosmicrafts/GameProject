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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName()
    { 
        if(inputNameField.text != null)
        {
            string playerName = inputNameField.text;
            PlayerPrefs.SetString("AccounName", playerName);
            SceneManager.LoadScene(mainScene);

        }


    }
}
