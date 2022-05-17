using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_2 : GameCharacter
{
    //Reduce time to recover shields to units
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        unit.ShieldDelay *= 0.75f;
    }
}
