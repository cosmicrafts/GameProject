using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UIReward : MonoBehaviour
{
    
    [SerializeField] Button[] buttonsNFT;
    [SerializeField] Image[] imagesNFT;
    [SerializeField] TMP_Text[] textsNFT;
    
    [SerializeField] Image imageShowReward;
    [SerializeField] TMP_Text textNameReward;
    [SerializeField] TMP_Text textTypeReward;
    [SerializeField] private GameObject UIShowReward;
    
    [Header("Multiple Rewards")]
    [SerializeField] private GameObject UIShowMultipleReward;
    [SerializeField] private GameObject ContentPrefabs;
    [SerializeField] private GameObject NFTPrefab;
    [SerializeField] private TMP_Text NFTName;
    [SerializeField] private Image NFTImage;
    
    [Header("Buttons")]
    [SerializeField] private GameObject ButtonOpenAll;
    [SerializeField] private GameObject ButtonNext;

    private List<NFTsUnit> Cards = new List<NFTsUnit>();
    
    public void ClaimNFT(int index)
    {
        buttonsNFT[index].interactable = false;
        Debug.Log("ClaimNFT: " + index);
        GameNetwork.JSClaimNft(index);
        LoadingPanel.instance.ActiveLoadingPanel();

        bool flagButtonsAreInteractable = false;
        foreach (Button button in buttonsNFT)
        {
            if (button.IsInteractable())
            {
                flagButtonsAreInteractable = true;
            }
        }

        if (!flagButtonsAreInteractable)
        {
            ButtonOpenAll.SetActive(false);
            ButtonNext.SetActive(true);
        }
    }
    
    public void ClaimAllNFT()
    {
        int index = 0; string indexArray = "";
        foreach (Button button in buttonsNFT)
        {
            if (button.IsInteractable())
            {
                buttonsNFT[index].interactable = false;
                indexArray += index +",";
            }
            index++;
        }
        indexArray = indexArray.TrimEnd(',');
        
        Debug.Log("ClaimAllNFT: " + indexArray);
        GameNetwork.JSClaimAllNft(indexArray);
        LoadingPanel.instance.ActiveLoadingPanel();
        ButtonOpenAll.SetActive(false);
        ButtonNext.SetActive(true);
    }

    
    public void OnClaimNFTCompleted(string jsonData)
    {
        Debug.Log("Recibí: " + jsonData);
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsUnit>>(jsonData));
        Debug.Log("Cards List Lenght: " + Cards.Count);
        if (Cards.Count == 1)
        {
            StartCoroutine(LoadNFTsIcons());
        }
        else if(Cards.Count > 1)
        {
            StartCoroutine(LoadAllNFTsIcons());
        }
        else
        {
            Debug.Log("Error NTF List is 0");
        }
        
    }

    private IEnumerator LoadNFTsIcons()
    {
        Debug.Log("Entre a la coroutine: LoadNFTsIcons");
        
        foreach (NFTsUnit card in Cards)
        {
            textsNFT[card.ID].text = card.Name;
            textNameReward.text = card.Name;

            Debug.Log(card.EntType);
            if (card.EntType == 0)
            {
                textTypeReward.text = "YOU RECEIVED A HERO!";
            }
            else
            {
                textTypeReward.text = "YOU RECEIVED A SPACESHIP!";
            }
            
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
    
    private IEnumerator LoadAllNFTsIcons()
    {
        Debug.Log("Entre a la coroutine: LoadAllNFTsIcons");
        
        foreach (NFTsUnit card in Cards)
        {
            textsNFT[card.ID].text = card.Name;
            NFTName.text = card.Name;
            
            if (!string.IsNullOrEmpty(card.IconURL))
            {
                
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(card.IconURL);
                yield return www.SendWebRequest();
                Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                imagesNFT[card.ID].sprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                NFTImage.sprite = imagesNFT[card.ID].sprite;
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
