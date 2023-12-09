using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public string description;
    public int level;
    public int quantity;  // New field for quantity

    // UI references
    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI quantityText;  // New Text field for quantity
    public Button levelUpButton;

    public InventoryItem(string name, string description, int level, int quantity, Image itemImage, TextMeshProUGUI nameText, TextMeshProUGUI descriptionText, TextMeshProUGUI levelText, TextMeshProUGUI quantityText, Button levelUpButton)
    {
        this.name = name;
        this.description = description;
        this.level = level;
        this.quantity = quantity;  // Initialize quantity

        this.itemImage = itemImage;
        this.nameText = nameText;
        this.descriptionText = descriptionText;
        this.levelText = levelText;
        this.quantityText = quantityText;
        this.levelUpButton = levelUpButton;

        this.levelUpButton.onClick.AddListener(LevelUp);
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update the UI components with the current data
        if (nameText != null) nameText.text = name;
        if (descriptionText != null) descriptionText.text = description;
        if (levelText != null) levelText.text = "Level: " + level.ToString();
        if (quantityText != null) quantityText.text = "Qty: " + quantity.ToString();
    }

    public void LevelUp()
    {
        // Increment level and update UI
        level++;
        UpdateUI();
    }

    // Methods to handle receiving items and transferring items (used in event system)
    public void Receive(int amount)
    {
        quantity += amount;
        UpdateUI();
    }

    public bool Transfer(int amount)
    {
        if (quantity >= amount)
        {
            quantity -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }
}
