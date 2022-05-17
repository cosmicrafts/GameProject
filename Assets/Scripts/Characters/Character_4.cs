using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_4 : GameCharacter
{
    //Add 2 shield points and movement speed to units
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
