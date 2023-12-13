using UnityEngine;
using TMPro;

public class NumberFormatterManager : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; // Reference to the TextMeshProUGUI component

    private string[] suffixes = {"", "K", "M", "B", "T"};

    private float currentValue; // The current value to be formatted

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the same GameObject.");
        }

        // Set an initial value (you can set this value as needed)
        SetNumber(0f);
    }

    private void UpdateText()
    {
        int index = 0;
        float formattedNumber = currentValue;

        while (formattedNumber >= 1000 && index < suffixes.Length - 1)
        {
            formattedNumber /= 1000;
            index++;
        }

        if (textMeshPro != null)
        {
            textMeshPro.text = $"{formattedNumber:F2}{suffixes[index]}";
        }
    }

    public void SetNumber(float newValue)
    {
        currentValue = newValue;
        UpdateText();
    }
}
