using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_1 : GameCharacter
{
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.BulletDamage = (int)((float)shooter.BulletDamage * 1.25f);
            shooter.CoolDown *= 0.75f;
        }
        Ship ship = unit.GetComponent<Ship>();
        if (ship != null)
        {
            ship.MaxSpeed *= 1.25f;
        }
    }
}
