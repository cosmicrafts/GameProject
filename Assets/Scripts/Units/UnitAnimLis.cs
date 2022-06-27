using UnityEngine;

/*
 * The animation controller script for units
 */

public class UnitAnimLis : MonoBehaviour
{
    //Unit data reference
    Unit MyUnit;

    // Start is called before the first frame update
    void Start()
    {
        //Get unit data
        MyUnit = transform.parent.GetComponent<Unit>();
    }

    //Called when the deth animation ends
    public void AE_EndDeath()
    {
        //Kill the unit
        MyUnit.DestroyUnit();
    }

    //Called an explosion effect
    public void AE_BlowUpUnit()
    {
        //Kill the unit
        MyUnit.BlowUpEffect();
    }
}
