

public class UserGeneral : User
{
    public int Level { get; set; }

    public int Xp { get; set; }

    public string Icon { get; set; }

    public int GetNextXpGoal()
    {
        return 100 * Level;
    }
}
