using System.Collections;
using System.Collections.Generic;

public class UserProgress
{
    int Level { get; set; }

    int Xp { get; set; }

    int XpForLeveUp { get; set; }

    public void InitValues(int level, int xp)
    {
        Level = level;
        Xp = xp;
        XpForLeveUp = GetNextXpGoal();
    }

    public int GetNextXpGoal()
    {
        return 100 * Level;
    }

    public int GetXp()
    {
        return Xp;
    }

    public int GetLevel()
    {
        return Level;
    }

    public void AddXp(int plus)
    {
        int required = XpForLeveUp - Xp;
        int bucket = plus;

        while (bucket >= required)
        {
            bucket -= required;
            LevelUp();
            required = XpForLeveUp;
        }

        Xp += bucket;
    }

    public void LevelUp()
    {
        Level++;
        Xp = 0;
        XpForLeveUp = GetNextXpGoal();
    }
}
