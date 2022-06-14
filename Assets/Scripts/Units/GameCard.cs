using UnityEngine;

/*
 * This is the basic game card data used on the units and spells prefabs
 */

//Type of the prefab
public enum CardType
{
    Unit,
    Spell
}

public class GameCard : MonoBehaviour
{
    public string NftsKey;

    public string Name;

    public int EnergyCost;

    public Sprite Icon;

    public CardType cardType;
}
