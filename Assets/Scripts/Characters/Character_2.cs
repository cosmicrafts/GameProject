using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_2 : GameCharacter
{
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        unit.ShieldDelay *= 0.75f;
    }
}
