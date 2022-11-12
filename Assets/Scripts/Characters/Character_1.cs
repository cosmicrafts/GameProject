/* 
 * This is the first in-game character controller 
 * Wegnar
 */
public class Character_1 : GameCharacter
{
    //Add dmg and speed attack to units
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        Shooter shooter = unit.GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.BulletDamage = (int)((float)shooter.BulletDamage * 1.1f);
            shooter.CoolDown *= 0.9f;
        }
    }
}
