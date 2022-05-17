using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_3 : GameCharacter
{
    // Player begins with more energy
    void Start()
    {
        GameMng.P.MaxEnergy += 5f;
    }

    //Add 1 shield points and more range attack to units
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        unit.Shield += 1;
        unit.SetMaxShield(unit.Shield);
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.RangeDetector += 1f; 
        }
    }
}
