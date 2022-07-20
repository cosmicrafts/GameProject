using UnityEngine;
using UnityEngine.UI;

/*
 * This script manages the UI of the ships and stations
 * Shows HP Bars, Shields, etc.
 */

public class UIUnit : MonoBehaviour
{
    //The main game object canvas reference
    public GameObject Canvas;

    //HP lines image
    //public Image HpLine;

    //HP bar image
    public Image Hp;
    public Image GHp;

    //Shield lines image
    //public Image ShieldLine;

    //Shield bar image
    public Image Shield;
    public Image GShield;

    //The main camera of the game
    Camera MainCamera;

    //Dmg diferences values
    float GhostHp;
    float GhostSH;

    private void Update()
    {
        //The UI always look at the camera
        transform.LookAt(transform.position + MainCamera.transform.rotation * Vector3.back, MainCamera.transform.rotation * Vector3.up);
        //Lerp Ghost Bars
        GhostHp = Mathf.Lerp(GhostHp, Hp.fillAmount, Time.deltaTime * 10f);
        GhostSH = Mathf.Lerp(GhostSH, Shield.fillAmount, Time.deltaTime * 10f);
        GHp.fillAmount = GhostHp;
        GShield.fillAmount = GhostSH;
    }

    //Init the shield and hp bars
    public void Init(int maxhp, int maxshield)
    {
        //Set the main camera
        MainCamera = Camera.main;

        //Init Ghost Bars
        GhostHp = maxhp;
        GhostSH = maxshield;
        GHp.color = Color.yellow;
        GShield.color = Color.gray;

        //Set the number of hp lines
        //int maxLines = maxhp / 4;

        //Instantiate the hp lines
        //for (int i = 0; i < maxLines; i++)
        //    Instantiate(HpLine, HpLine.transform.parent);

        //Check for shield points
        //if (maxshield > 0)
        //{
        //    //Set the number of shield lines
        //    //maxLines = maxshield / 4;
        //    //Instantiate the shield lines
        //    //for (int i = 0; i < maxLines; i++)
        //    //    Instantiate(ShieldLine, ShieldLine.transform.parent);
        //} else //The unit doesn´t have shield so we hide these elements
        //{
        //    Shield.transform.parent.gameObject.SetActive(false);
        //}
    }

    //Set the hp amount
    public void SetHPBar(float porcent)
    {
        Hp.fillAmount = porcent;
    }

    //Set the shield amount
    public void SetShieldBar(float porcent)
    {
        Shield.fillAmount = porcent;
    }

    //Set the bars colors (depending if is an enemy unit)
    public void SetColorBars(bool imEnnemy)
    {
        Hp.color = GameMng.UI.GetHpBarColor(imEnnemy);
        Shield.color = GameMng.UI.GetShieldBarColor(imEnnemy);
    }

    //Hide the UI
    public void HideUI()
    {
        Canvas.SetActive(false);
    }
}
