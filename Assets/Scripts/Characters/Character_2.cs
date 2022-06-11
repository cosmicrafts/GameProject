/* 
 * This is the 2° in-game character controller 
 * Dofshlu
 */
public class Character_2 : GameCharacter
{
    //Reduce time to recover shields to units
    public override void DeployUnit(Unit unit)
    {
        base.DeployUnit(unit);
        unit.ShieldDelay *= 0.75f;
    }
}
