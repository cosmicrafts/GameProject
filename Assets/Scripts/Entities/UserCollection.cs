using System.Collections;
using System.Collections.Generic;

public class UserCollection
{
    public List<NFTsCard> Deck;

    public List<NFTsCard> Cards;

    public List<NFTsCharacter> Characters;

    public void InitCollection()
    {
        Deck = new List<NFTsCard>();
        Cards = new List<NFTsCard>();
        Characters = new List<NFTsCharacter>();
    }
}
