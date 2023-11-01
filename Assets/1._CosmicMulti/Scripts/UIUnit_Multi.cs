using UnityEngine;
using UnityEngine.UI;

/*
 * This script manages the UI of the ships and stations
 * Shows HP Bars, Shields, etc.
 */

public class UIUnit_Multi : MonoBehaviour
{
    //The main game object canvas reference
    public GameObject Canvas;

 
    //HP bar image
    public Image Hp;
    public Image GHp;

    //Shield bar image
    public Image Shield;
    public Image GShield;

    public float DifDmgSpeed = 10f;
    public Color DifHpColor = Color.yellow;
    public Color DifShieldColor = Color.gray;
    
    //Dmg diferences values
    float GhostHp;
    float GhostSH;
    
	Quaternion originalRotation;
    Camera mainCamera;
    
    void Start()
    {
        originalRotation = transform.rotation;
        mainCamera = Camera.main;
        GHp.color = DifHpColor;
        GShield.color = DifShieldColor;
    }
    private void Update()
    {
        //The UI always look at the camera
        transform.rotation = mainCamera.transform.rotation * originalRotation;
        //Lerp Ghost Bars
        GhostHp = Mathf.Lerp(GhostHp, Hp.fillAmount, Time.deltaTime * DifDmgSpeed);
        GhostSH = Mathf.Lerp(GhostSH, Shield.fillAmount, Time.deltaTime * DifDmgSpeed);
        GHp.fillAmount = GhostHp;
        GShield.fillAmount = GhostSH;
    }
    
    public void SetHPBar    (float porcent) { Hp.fillAmount = porcent; }
    public void SetShieldBar(float porcent) { Shield.fillAmount = porcent; }
    
    public void SetColorBars(Color color)
    {
        Hp.color = color;
        Shield.color = color;
    }

    public void HideUI() { Canvas.SetActive(false); }




}
