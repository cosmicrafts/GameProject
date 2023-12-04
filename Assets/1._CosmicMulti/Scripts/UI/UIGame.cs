using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public delegate void CreateHeroHandler(int index, SimpleVector2 position, IEnumerator wait);

public class UIGame : MonoBehaviour
{
    public CreateHeroHandler OnCreateHero;
    
    [SerializeField] private UIGameCard[] heroButtons;
    [SerializeField] private Text pausedText;
    [SerializeField] private Text winLoseText;
    [SerializeField] private LayerMask uiMask;
    
    [SerializeField] private Transform heroSpawnPoint;
    private GameObject heroObjPrev;
    [HideInInspector] public List<GameObject> previewMeshs = new List<GameObject>();
    [HideInInspector] public List<Material> PreviewMaterials = new List<Material>();
    [HideInInspector] public List<int> CardEnergyCost = new List<int>();
    [HideInInspector] public List<Sprite> CardSprites = new List<Sprite>();
    
    [Header("EnergyUI")]
    public TMP_Text EnergyLabel;
    public Image EnergyBar;
   
    [Header("PlayersUI")]
    public UIGamePlayer[] Players = new UIGamePlayer[2];
    
    
    public float HeroCreateTime { get; set; }
    private int selectedHeroIndex = -1;
    private int groupIndex;

    private void Awake()
    {
        winLoseText.gameObject.SetActive(false);
        
    }

    public void SetGroupIndex(int index)
    {
        groupIndex = index;
        heroSpawnPoint.RotateAround (transform.position, transform.up, 180f * index);

        if (index == 0) { Players[0].InitInfo(GlobalGameData.Instance.GetUserData()); Players[1].InitInfo(GlobalGameData.Instance.GetVsUserData()); }
                   else { Players[1].InitInfo(GlobalGameData.Instance.GetUserData()); Players[0].InitInfo(GlobalGameData.Instance.GetVsUserData()); }
        
        
        foreach (UIGameCard heroButton in heroButtons)
        {
            heroButton.OnDown += (UIGameCard btn) =>
            {
                heroButton.SetSelection(true);
                selectedHeroIndex = heroButton.Index + (groupIndex * 8);
                CreatePreviewObj(heroButton.Index + (groupIndex * 8) );
            };
            //Fill Button UI INFO
            heroButton.EnergyCost = CardEnergyCost[heroButton.Index + (groupIndex * 8)];
            heroButton.TextCost.text = heroButton.EnergyCost.ToString();
            heroButton.SpIcon.sprite = CardSprites[heroButton.Index + (groupIndex * 8)];
        }
        
    }
    public void OnPaused(bool isOn) { pausedText.gameObject.SetActive(isOn); }

    public void OnWinOrLose(bool isWin) 
    {
        winLoseText.gameObject.SetActive(true);
        winLoseText.text = isWin ? "Win" : "Lose";
    }
    
    
    private void Update()
    {
        if (selectedHeroIndex != -1 )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 50f, uiMask) && (groupIndex == 0 && hit.point.z < 0 || groupIndex == 1 && hit.point.z > 0))
            {
                heroSpawnPoint.gameObject.SetActive(true);
                heroSpawnPoint.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x), 0.0f, Mathf.RoundToInt(hit.point.z));
               
                if (Input.GetMouseButtonUp(0))
                {
                    SimpleVector2 position = new SimpleVector2((int)hit.point.x, (int)hit.point.z);
                    OnCreateHero?.Invoke(selectedHeroIndex, position, OnSendInfoToDeployShip() );
                }
            }
            else
            {
                heroSpawnPoint.gameObject.SetActive(false);
            }
            if (Input.GetMouseButtonUp(0))
            {
                heroButtons[selectedHeroIndex % 8].SetSelection(false);
                heroSpawnPoint.gameObject.SetActive(false);
                selectedHeroIndex = -1;
            }
            
        }
    }
    
    public void UpdateEnergyUI(float energy, float maxEnergy)
    {
        EnergyLabel.text = ((int)energy).ToString(energy == maxEnergy ? "F0" : "F0"); //F1  F1"); 
        EnergyBar.fillAmount = energy / maxEnergy;

        foreach (UIGameCard heroButton in heroButtons)
        {
            heroButton.TextCost.color = energy >= heroButton.EnergyCost ? Color.white : Color.red;
            heroButton.enabled = energy >= heroButton.EnergyCost ? true : false;
        }
    }

    //Set the current preview from a game object
    public void CreatePreviewObj(int index)
    {
        if (heroObjPrev != null) { Destroy(heroObjPrev); }
        
        heroObjPrev = Instantiate(previewMeshs[index], heroSpawnPoint);

        Animator animationInPrefab = heroObjPrev.GetComponent<Animator>();
        if(animationInPrefab != null) { DestroyImmediate(animationInPrefab, true);}
        
        SkinnedMeshRenderer skinnedRenderer = heroObjPrev.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedRenderer)
        {  
            Material[] mats = skinnedRenderer.materials;
            mats[0] = PreviewMaterials[index];
            skinnedRenderer.materials = mats;
        }
        
    }

    IEnumerator OnSendInfoToDeployShip()
    {
        GameObject tempObjPreview = heroObjPrev; heroObjPrev = null;
        tempObjPreview.transform.SetParent(null);
        
        float time = 0.0f;
        float createTime = HeroCreateTime;
        while (time < createTime)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(tempObjPreview);
    }
    
}
