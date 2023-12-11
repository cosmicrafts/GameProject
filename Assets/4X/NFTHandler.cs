using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;

public class NFTHandler : MonoBehaviour
{
    [System.Serializable]
    public class NFTData
    {
        public string nftName;
        public string image;
        public string description;
        public Dictionary<string, string> balances; // Holds the quantity of each NFT for different addresses
    }

    [System.Serializable]
    public class NFTList
    {
        public List<NFTData> nfts; // List to store all NFT data
    }

    private Dictionary<string, NFTData> nftDictionary = new Dictionary<string, NFTData>();
    public GameObject nftTemplate; // Prefab for displaying NFTs

    void Start()
    {
        // Start loading NFT data asynchronously when the game starts
        StartCoroutine(LoadNFTDataAsync("4X/NFTs"));
    }

    IEnumerator LoadNFTDataAsync(string path)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(path);
        yield return request;

        // Check if the NFT data is successfully loaded
        if (request.asset is TextAsset fileData)
        {
            NFTList nftList = JsonConvert.DeserializeObject<NFTList>(fileData.text);
            ProcessNFTList(nftList);

            // Deactivate the NFT template after processing the list
            nftTemplate.SetActive(false);
        }
        else
        {
            // Log an error if the data fails to load
            Debug.LogError("Failed to load NFT data.");
        }
    }

    void ProcessNFTList(NFTList nftList)
    {
        foreach (var nftData in nftList.nfts)
        {
            // Store NFT data in a dictionary for easy access
            nftDictionary[nftData.nftName] = nftData;

            // Instantiate a UI element for each NFT
            GameObject nftUI = Instantiate(nftTemplate, transform);
            nftUI.SetActive(true); // Ensure the instantiated object is active
            UpdateNFTUI(nftUI, nftData);
        }
    }

    void UpdateNFTUI(GameObject nftUI, NFTData nftData)
    {
        // Update the UI elements with NFT data
        nftUI.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = nftData.nftName;
        nftUI.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = nftData.description;
        nftUI.transform.Find("Image").GetComponent<Image>().sprite = LoadSprite(nftData.image);
        nftUI.transform.Find("Balances").GetComponent<TextMeshProUGUI>().text = "Quantity: " + nftData.balances["Address1"];
    }

    Sprite LoadSprite(string imagePath)
    {
        // Load and return the sprite for the NFT image
        return Resources.Load<Sprite>(imagePath);
    }
}
