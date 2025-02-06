using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
namespace CosmicraftsSP {
public class UIRewardMenu : MonoBehaviour
{
    
    [SerializeField] Button buttonNFT;
    
    [Header("Multiple Rewards")]
    [SerializeField] private GameObject UIShowMultipleReward;
    [SerializeField] private GameObject ContentPrefabs;
    [SerializeField] private GameObject NFTPrefab;
    [SerializeField] private TMP_Text NFTName;
    [SerializeField] private Image NFTImage;
    
    private List<NFTsUnit> Cards = new List<NFTsUnit>();
    
    public void ClaimReward(int index)
    {
        buttonNFT.interactable = false;
        Debug.Log("ClaimNFT: " + index);
        GameNetwork.JSClaimNft(index); //Cambiarla por otra para evitar conflictos
        LoadingPanel.instance.ActiveLoadingPanel();
        
    }
    public void OnClaimNFTCompleted(string jsonData)
    {
        Debug.Log("Recibí: " + jsonData);
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsUnit>>(jsonData));
        Debug.Log("Cards List Lenght: " + Cards.Count);
        if (Cards.Count >= 1)
        {
            StartCoroutine(LoadAllNFTsIcons());
        }
        else
        {
            Debug.Log("Error NTF List is 0");
        }
    }
    
    private IEnumerator LoadAllNFTsIcons()
    {
        Debug.Log("Entre a la coroutine: LoadAllNFTsIcons");
        
        foreach (NFTsUnit card in Cards)
        {
            NFTName.text = card.Name;
            
            if (!string.IsNullOrEmpty(card.IconURL))
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(card.IconURL);
                yield return www.SendWebRequest();
                Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                NFTImage.sprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            
            Instantiate(NFTPrefab, ContentPrefabs.transform).SetActive(true);
        }
        
        Cards.Clear();
        UIShowMultipleReward.SetActive(true);
        LoadingPanel.instance.DesactiveLoadingPanel();
    }

    public void GoToMenu(string scene)
    {
        SceneManager.LoadScene(scene);
        GameNetwork.JSGoToMenu();
    }
    
    
}
}