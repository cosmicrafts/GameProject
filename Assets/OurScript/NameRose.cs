using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameRose : MonoBehaviour
{
    public Text nametext;
    // Start is called before the first frame update
    void Start()
    {
        nametext.text = PlayerPrefs.GetString("Account");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
