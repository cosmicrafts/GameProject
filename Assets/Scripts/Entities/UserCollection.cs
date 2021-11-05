using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        if (GameData.DebugMode)
        {
            NFTsCharacter default_character = new NFTsCharacter()
            {
                KeyId = "chr1",
                Name = "Wegnar",
                Icon = "Character_1"
            };
            Characters.Add(default_character);
            GameData.SetUserCharacter(default_character);
            for(int i = 1; i<11; i++)
            {
                Cards.Add(new NFTsCard()
                {
                    KeyId = $"Unit_{i}",
                    Name = $"Unit {i}",
                    EnergyCost = 3,
                    Icon = $"I_Ship_{i}",
                    Rarity = 1
                });
            }
            for (int i = 1; i < 2; i++)
            {
                Cards.Add(new NFTsCard()
                {
                    KeyId = $"Station_{i}",
                    Name = $"Station {i}",
                    EnergyCost = 3,
                    Icon = $"I_Sta_{i}",
                    Rarity = 1
                });
            }
            Deck = Cards.Take(8).ToList();
        }
    }
}
