using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnit : MonoBehaviour
{
    public GameObject Canvas;

    public Image HpLine;

    public Image Hp;

    public Image ShieldLine;

    public Image Shield;

    Camera MainCamera;

    private void Update()
    {
        transform.LookAt(transform.position + MainCamera.transform.rotation * Vector3.back, MainCamera.transform.rotation * Vector3.up);
    }

    public void Init(int maxhp, int maxshield)
    {
        MainCamera = Camera.main;

        for (int i = 0; i < maxhp; i++)
            Instantiate(HpLine, HpLine.transform.parent);

        if (maxshield > 0)
        {
            for (int i = 0; i < maxshield; i++)
                Instantiate(ShieldLine, ShieldLine.transform.parent);
        } else
        {
            Shield.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetHPBar(float porcent)
    {
        Hp.fillAmount = porcent;
    }

    public void SetShieldBar(float porcent)
    {
        Shield.fillAmount = porcent;
    }

    public void HideUI()
    {
        Canvas.SetActive(false);
    }
}
