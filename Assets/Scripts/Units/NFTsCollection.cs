using System.Collections.Generic;


public enum Factions
{
    Neutral,
    Alliance,
    Spirats
}

public class NFTsCollection
{
    public List<NFTsCard> AllCards;

    public List<NFTsCharacter> AllCharacters;

    public void InitCollection()
    {
        AllCards = new List<NFTsCard>();
        AllCharacters = new List<NFTsCharacter>();
    }
}
