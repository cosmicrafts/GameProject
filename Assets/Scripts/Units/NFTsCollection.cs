using System.Collections.Generic;


public enum Factions
{
    Neutral,
    Alliance,
    Spirats
}

public class NFTsCollection
{
    public static char[] NFTsPrefix = new char[4] { 'C', 'H', 'S', 'U' };

    public static Dictionary<Factions,string> NFTsFactionsPrefixs = new Dictionary<Factions, string> {
        {Factions.Neutral, "NEU"},
        {Factions.Alliance, "ALL"},
        {Factions.Spirats, "SPI"}
    };

    public List<NFTsCard> AllCards;

    public List<NFTsCharacter> AllCharacters;

    public void InitGlobalCollection()
    {
        AllCards = new List<NFTsCard>();
        AllCharacters = new List<NFTsCharacter>();
    }
}
