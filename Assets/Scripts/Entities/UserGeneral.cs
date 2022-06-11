/*
 * This is a class used for unifys the basic data and progression of the player
 * Only used for multiplayer comunication
 */

public class UserGeneral : User
{
    public int Level { get; set; }

    public int Xp { get; set; }

    public string CharacterKey { get; set; }

    public int GetNextXpGoal()
    {
        return 100 * Level;
    }
}
