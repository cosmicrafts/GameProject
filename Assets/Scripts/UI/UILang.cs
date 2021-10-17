﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILang : MonoBehaviour
{
    public string ID = "None";
    bool okstart = false;

    // Use this for initialization
    void Start()
    {
        SetMyText();
        okstart = true;
    }

    public void SetMyText()
    {
        GetComponent<Text>().text = Lang.GetText(ID);
    }

    void OnEnable()
    {
        if (okstart)
            SetMyText();
    }
}
