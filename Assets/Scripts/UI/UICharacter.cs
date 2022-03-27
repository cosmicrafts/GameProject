using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    private NFTsCharacter Data;

    public Image MyAvatar;
    public Text MyName;
    public Animator MyAnim;

    [HideInInspector]
    public float AlphaFactor;
    [HideInInspector]
    public float DeltaFactor;
    [HideInInspector]
    public Scrollbar RefScroll;
    [HideInInspector]
    public float CurrentDelta;

    // Start is called before the first frame update
    void Start()
    {
        MyAnim.SetFloat("AlphaAmount", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDelta();
    }

    public void UpdateDelta()
    {
        CurrentDelta = Mathf.Abs((RefScroll.value - AlphaFactor) * DeltaFactor);
        MyAnim.SetFloat("AlphaAmount", CurrentDelta > 1f ? 1f : CurrentDelta);
    }

    public void SetData(NFTsCharacter data)
    {
        Data = data;
        MyAvatar.sprite = ResourcesServices.LoadCharacterIcon(Data.Icon);
        MyName.text = Lang.GetEntityName(data.KeyId);
    }

    public NFTsCharacter GetData()
    {
        return Data;
    }

    public void SelectChar()
    {
        MyName.color = Color.yellow;
    }

    public void DeselectChar()
    {
        MyName.color = Color.white;
    }
}
