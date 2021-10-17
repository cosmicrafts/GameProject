using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimLis : MonoBehaviour
{
    Unit MyUnit;

    // Start is called before the first frame update
    void Start()
    {
        MyUnit = transform.parent.GetComponent<Unit>();
    }

    public void AE_EndDeath()
    {
        MyUnit.DestroyUnit();
    }
}
