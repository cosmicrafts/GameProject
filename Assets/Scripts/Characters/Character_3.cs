using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_3 : GameCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        GameMng.P.MaxEnergy += 5f;
    }

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
