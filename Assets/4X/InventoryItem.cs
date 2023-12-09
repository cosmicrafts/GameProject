using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    private class ItemData
    {
        public string name;
        public string description;
        public int level;
        public int quantity;
    }

    [System.Serializable]
    private class InventoryData
    {
        public List<ItemData> items;
    }

    public List<InventoryItem> items = new List<InventoryItem>();

    void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        TextAsset fileData = Resources.Load<TextAsset>("4X/Inventory");
        if (fileData != null)
        {
            InventoryData data = JsonUtility.FromJson<InventoryData>(fileData.text);
            foreach (var itemData in data.items)
            {
                // Find the GameObject and its components
                GameObject itemGO = GameObject.Find(itemData.name);
                if (itemGO != null)
                {
                    Image itemImage = itemGO.GetComponentInChildren<Image>();
                    TextMeshProUGUI nameText = itemGO.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI descriptionText = itemGO.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI levelText = itemGO.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI quantityText = itemGO.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();
                    Button levelUpButton = itemGO.transform.Find("LvlUp_Btn").GetComponent<Button>();

                    // Create a new InventoryItem and add it to the list
                    items.Add(new InventoryItem(itemData.name, itemData.description, itemData.level, itemData.quantity, itemImage, nameText, descriptionText, levelText, quantityText, levelUpButton));
                }
                else
                {
                    Debug.LogError("GameObject not found for: " + itemData.name);
                }
            }
        }
        else
        {
            Debug.LogError("JSON file not found in Resources folder.");
        }
    }
}
