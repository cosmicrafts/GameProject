using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_4 : GameCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        unit.Shield += 2;
        unit.SetMaxShield(unit.Shield);
        Ship ship = unit.GetComponent<Ship>();
        if (ship != null)
        {
            ship.MaxSpeed *= 1.25f;
        }
    }
}
