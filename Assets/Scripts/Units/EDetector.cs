using UnityEngine;

/*
 * This is the unit enemy detector
 */

public class EDetector : MonoBehaviour
{
    // Unit data reference
    public Unit MyUnit;

    // Shooter script reference
    public Shooter MyShooter;

    // New enemy detected (add to enemies list)
    private void OnTriggerEnter(Collider other)
    {
        // Check if the detected object is a unit
        if (other.CompareTag("Unit"))
        {
            // Check if the unit is an enemy unit and still alive
            Unit OtherUnit = other.gameObject.GetComponent<Unit>();
            if (OtherUnit != null && !OtherUnit.IsMyTeam(MyUnit.MyTeam) && !OtherUnit.GetIsDeath())
            {
                // Add the unit to the enemies list
                MyShooter.AddEnemy(OtherUnit);
            }
        }
    }

    // Enemy out of range (delete from enemies list)
    private void OnTriggerExit(Collider other)
    {
        // Check if the detected object is a unit
        if (other.CompareTag("Unit"))
        {
            // Check if the unit is an enemy unit
            Unit OtherUnit = other.gameObject.GetComponent<Unit>();
            if (OtherUnit != null && !OtherUnit.IsMyTeam(MyUnit.MyTeam))
            {
                // Delete the unit from the enemies list
                MyShooter.RemoveEnemy(OtherUnit);
            }
        }
    }

    // Clean up destroyed or inactive units
    public void CleanUpEnemies()
    {
        MyShooter.CleanEnemies();
    }
}
