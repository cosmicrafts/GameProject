using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logout : MonoBehaviour
{
    public GameObject LoginForm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogOut()
    {
        GameData.PlayerUser = null;
        LoginForm.SetActive(true);
        gameObject.SetActive(false);
    }
}
