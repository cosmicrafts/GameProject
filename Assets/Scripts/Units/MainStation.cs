using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStation : MonoBehaviour
{
    Unit MyUnit;

    private void Start()
    {
        MyUnit = GetComponent<Unit>();
    }
}
