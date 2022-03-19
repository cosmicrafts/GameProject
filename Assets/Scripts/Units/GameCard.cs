using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
