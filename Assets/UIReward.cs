using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Image = UnityEngine.UI.Image;

public class UIReward : MonoBehaviour
{

    [SerializeField] Image[] imagesNFT;
    [SerializeField] TMP_Text[] textsNFT;
    
    [SerializeField] Image imageShowReward;
    [SerializeField] TMP_Text textShowReward;
    [SerializeField] private GameObject UIShowReward;
    
    public List<NFTsCard> Cards;
    
    
        
    public void ClaimNFT(int index)
    {
        Debug.Log("ClaimNFT: " + index);
        GameNetwork.JSClaimNft(index);
        LoadingPanel.instance.ActiveLoadingPanel();
    }
    
    public void OnClaimNFTCompleted(string jsonData)
    {
        Debug.Log("Recibí: " + jsonData);
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsUnit>>(jsonData));
        StartCoroutine(LoadNFTsIcons());
    }

    private IEnumerator LoadNFTsIcons()
    {
        Debug.Log("Entre a la coroutine");
        
        foreach (NFTsCard card in Cards)
        {
            textsNFT[card.ID].text = card.Name;
            textShowReward.text = card.Name;
            
            if (!string.IsNullOrEmpty(card.IconURL))
            {
                
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(card.IconURL);
                yield return www.SendWebRequest();
                Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                imagesNFT[card.ID].sprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                imageShowReward.sprite = imagesNFT[card.ID].sprite;
            }
        }
        
        Cards.Clear();
        UIShowReward.SetActive(true);
        LoadingPanel.instance.DesactiveLoadingPanel();
    }
    
    
    
    
}
