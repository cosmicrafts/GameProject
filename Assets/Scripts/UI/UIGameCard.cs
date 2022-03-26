using UnityEngine;
using UnityEngine.UI;

public class UIGameCard : MonoBehaviour
{
    // Start is called before the first frame update

    public int IdCardDeck = 0;

    public int EnergyCost = 99;

    public Text TextCost;

    public Image SpIcon;

    public GameObject Selection;

    public void SetSelection(bool selected)
    {
        Selection.SetActive(selected);
    }
}
