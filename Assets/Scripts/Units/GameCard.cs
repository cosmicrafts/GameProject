﻿using UnityEngine;

/*
 * This is the basic game card data used on the units and spells prefabs
 */

public class GameCard : MonoBehaviour
{
    public string Alias;

    [Range(0,10)]
    public int DefaultCost;
}
