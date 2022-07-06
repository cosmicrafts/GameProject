using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIReward : MonoBehaviour
{

    [SerializeField] Image[] imagesNFT;
    [SerializeField] TMP_Text[] textsNFT;
    
    [SerializeField] Image imageShowReward;
    [SerializeField] TMP_Text textShowReward;
    [SerializeField] private GameObject UIShowReward;
        
    public void ClaimNFT(int index)
    {
        GameNetwork.JSClaimNft(index);
        LoadingPanel.instance.ActiveLoadingPanel();
    }
    
    public void OnClaimNFTCompleted(int index, string name, string urlImage)
    {
        UIShowReward.SetActive(true);
        //imagesNFT[index].sprite = GetURLImage;
        textShowReward.text = name;
        //imagesNFT[index].sprite = GetURLImage;
        textsNFT[index].text = name;
        
        
        LoadingPanel.instance.DesactiveLoadingPanel();
    }
    
    
    
}
