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

        int maxLines = maxhp / 4;

        for (int i = 0; i < maxLines; i++)
            Instantiate(HpLine, HpLine.transform.parent);

        if (maxshield > 0)
        {
            maxLines = maxshield / 4;
            for (int i = 0; i < maxLines; i++)
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

    public void SetColorBars(bool imEnnemy)
    {
        Hp.color = GameMng.UI.GetHpBarColor(imEnnemy);
        Shield.color = GameMng.UI.GetShieldBarColor(imEnnemy);
    }

    public void HideUI()
    {
        Canvas.SetActive(false);
    }
}
