/* 
 * This is the first in-game character controller 
 * Wegnar
 */
public class Character_1 : GameCharacter
{
    public override void DeployUnit()
    {
        base.DeployUnit();
    }
}

//unit.ShieldDelay *= 0.75f; //Reduce time to recover shields to units
//shooter.BulletDamage = (int)((float)shooter.BulletDamage * 1.25f); //Increment Damage

// Player begins with more energy
        /*void Start()
        {
            GameMng.P.MaxEnergy += 5f;
        }*/