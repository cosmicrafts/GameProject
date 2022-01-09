using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDetector : MonoBehaviour
{
    public Unit MyUnit;

    public Shooter MyShooter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit OtherUnit = other.gameObject.GetComponent<Unit>();
            if (!OtherUnit.IsMyTeam(MyUnit.MyTeam) && !OtherUnit.GetIsDeath())
            {
                MyShooter.AddEnemy(OtherUnit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit OtherUnit = other.gameObject.GetComponent<Unit>();
            if (!OtherUnit.IsMyTeam(MyUnit.MyTeam))
            {
                MyShooter.RemoveEnemy(OtherUnit);
            }
        }
    }
}
