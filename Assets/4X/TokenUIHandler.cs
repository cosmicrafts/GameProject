using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class TokenUIHandler : MonoBehaviour
{
    [System.Serializable]
    public class TokenData
    {
        public string tokenName;
        public string tokenSymbol;
        public int decimals;
        public string totalSupply;
        public Dictionary<string, string> balances;
    }

    [System.Serializable]
    public class TokenList
    {
        public List<TokenData> tokens;
    }

    private Dictionary<string, TokenData> tokenDictionary = new Dictionary<string, TokenData>();
    private Dictionary<string, GameObject> tokenUIInstances = new Dictionary<string, GameObject>();
    public GameObject tokenTemplate;

    void Start()
    {
        StartCoroutine(LoadTokenDataAsync("4X/Tokens"));
    }

    IEnumerator LoadTokenDataAsync(string path)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(path);
        yield return request;

        if (request.asset is TextAsset fileData)
        {
            Debug.Log("JSON data loaded asynchronously.");
            TokenList tokenList = JsonConvert.DeserializeObject<TokenList>(fileData.text);
            ProcessTokenList(tokenList);
            tokenTemplate.SetActive(false);
        }
        else
        {
            Debug.LogError($"Failed to load {path}.json asynchronously.");
        }
    }

    void ProcessTokenList(TokenList tokenList)
    {
        foreach (var tokenData in tokenList.tokens)
        {
            tokenDictionary[tokenData.tokenName] = tokenData;
            GameObject tokenUI = CreateTokenUI(tokenData);
            tokenUIInstances[tokenData.tokenName] = tokenUI;
        }
    }

    GameObject CreateTokenUI(TokenData tokenData)
    {
        GameObject tokenInstance = Instantiate(tokenTemplate, transform);
        tokenInstance.SetActive(true);
        UpdateTokenUI(tokenInstance, tokenData);
        return tokenInstance;
    }

    void UpdateTokenUI(GameObject tokenInstance, TokenData tokenData)
    {
        SetTextComponent(tokenInstance, "Name", tokenData.tokenName);
        SetTextComponent(tokenInstance, "Symbol", tokenData.tokenSymbol);
        SetBalanceComponent(tokenInstance, tokenData);
    }

    void SetTextComponent(GameObject parent, string childName, string text)
    {
        var textComponent = parent.transform.Find(childName)?.GetComponent<TextMeshProUGUI>();
        if (textComponent != null) textComponent.text = text;
        else Debug.LogError($"{childName} Text component not found for: {text}");
    }

    void SetBalanceComponent(GameObject parent, TokenData tokenData)
    {
        var balanceText = parent.transform.Find("Balance")?.GetComponent<TextMeshProUGUI>();
        if (balanceText != null && tokenData.balances != null)
        {
            if (tokenData.balances.TryGetValue("Address1", out string balance))
            {
                balanceText.text = balance;
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

    public void UpdateTokenData(string tokenName, string newBalance)
    {
        if (tokenDictionary.TryGetValue(tokenName, out TokenData tokenData))
        {
            tokenData.balances["Address1"] = newBalance;
            if (tokenUIInstances.TryGetValue(tokenName, out GameObject tokenUI))
            {
                UpdateTokenUI(tokenUI, tokenData);
            }
        }
        else
        {
            Debug.LogError("Token not found: " + tokenName);
        }
    }
}
