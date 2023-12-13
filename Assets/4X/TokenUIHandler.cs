using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;

public class TokenUIHandler : MonoBehaviour
{
    private string[] suffixes = {"", "K", "M", "B", "T"};
    // Define a class to hold token data
    [System.Serializable]
    public class TokenData
    {
        public string tokenName;
        public string tokenSymbol;
        public int decimals;
        public string totalSupply;
        public string imageUrl; // Path to the token's image
        public Dictionary<string, string> balances; // Balances associated with addresses
    }

    // Define a class to hold a list of TokenData
    [System.Serializable]
    public class TokenList
    {
        public List<TokenData> tokens;
    }

    private Dictionary<string, TokenData> tokenDictionary = new Dictionary<string, TokenData>();
    public GameObject tokenTemplate; // Prefab for displaying tokens

    void Start()
    {
        // Start loading token data asynchronously
        StartCoroutine(LoadTokenDataAsync("4X/Tokens"));
    }

    // Coroutine to load token data from a JSON file
    IEnumerator LoadTokenDataAsync(string path)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(path);
        yield return request;

        if (request.asset is TextAsset fileData)
        {
            TokenList tokenList = JsonConvert.DeserializeObject<TokenList>(fileData.text);
            ProcessTokenList(tokenList);
            tokenTemplate.SetActive(false); // Deactivate the template as it's only used for instantiation
        }
        else
        {
            Debug.LogError($"Failed to load {path}.json asynchronously.");
        }
    }

    // Process the list of tokens and create UI elements for each
    void ProcessTokenList(TokenList tokenList)
    {
        foreach (var tokenData in tokenList.tokens)
        {
            tokenDictionary[tokenData.tokenName] = tokenData;
            CreateAndDisplayTokenUI(tokenData);
        }
    }

    // Create and display UI elements for a given token
    void CreateAndDisplayTokenUI(TokenData tokenData)
    {
        GameObject tokenUI = Instantiate(tokenTemplate, transform);
        tokenUI.SetActive(true);
        UpdateTokenUI(tokenUI, tokenData);
    }

    // Update the UI elements with token data
    void UpdateTokenUI(GameObject tokenUI, TokenData tokenData)
    {
        SetTextComponent(tokenUI, "Name", tokenData.tokenName);
        SetTextComponent(tokenUI, "Symbol", tokenData.tokenSymbol);
        SetBalanceComponent(tokenUI, tokenData);
        SetImageComponent(tokenUI, "Image", tokenData.imageUrl); // Set the token image
    }

    // Helper method to set text components
    void SetTextComponent(GameObject parent, string childName, string text)
    {
        var textComponent = parent.transform.Find(childName)?.GetComponent<TextMeshProUGUI>();
        if (textComponent != null) textComponent.text = text;
        else Debug.LogError($"{childName} Text component not found for: {text}");
    }

    // Helper method to set the balance text
    void SetBalanceComponent(GameObject parent, TokenData tokenData)
    {
        var balanceText = parent.transform.Find("Balance")?.GetComponent<TextMeshProUGUI>();
        if (balanceText != null && tokenData.balances != null)
        {
            if (tokenData.balances.TryGetValue("Address1", out string balance))
            {
                // Format the balance using the formatting logic
                float formattedBalance = float.Parse(balance);
                int index = 0;

                while (formattedBalance >= 1000 && index < suffixes.Length - 1)
                {
                    formattedBalance /= 1000;
                    index++;
                }

                balanceText.text = $"{formattedBalance:F2}{suffixes[index]}";
            }
            else
            {
                balanceText.text = "Balance: Not available";
                Debug.LogError("Balance for Address1 not found or balances dictionary is null for: " + tokenData.tokenName);
            }
        }
        else
        {
            Debug.LogError("Balance Text component or balances dictionary not found for: " + tokenData.tokenName);
        }
    }


    // Helper method to set the image component
    void SetImageComponent(GameObject parent, string childName, string imagePath)
    {
        var imageComponent = parent.transform.Find(childName)?.GetComponent<Image>();
        if (imageComponent != null)
        {
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            if (sprite != null) imageComponent.sprite = sprite;
            else Debug.LogError($"Image not found at path: {imagePath}");
        }
        else
        {
            Debug.LogError($"{childName} Image component not found for: {imagePath}");
        }
    }

    // Public method to get the balance of a token
    public string GetTokenBalance(string tokenName)
    {
        if (tokenDictionary.TryGetValue(tokenName, out TokenData tokenData))
        {
            if (tokenData.balances.TryGetValue("Address1", out string balance))
            {
                return balance;
            }
        }
        return "0"; // Return 0 if token not found
    }

    // Public method to update token data and UI
    public void UpdateTokenData(string tokenName, string newBalance)
    {
        if (tokenDictionary.TryGetValue(tokenName, out TokenData tokenData))
        {
            tokenData.balances["Address1"] = newBalance;
            // Find and update the UI object
            foreach (Transform child in transform)
            {
                if (child.name == tokenName)
                {
                    UpdateTokenUI(child.gameObject, tokenData);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Token not found: " + tokenName);
        }
    }
}
