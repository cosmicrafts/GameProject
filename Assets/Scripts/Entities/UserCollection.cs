using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UserCollection
{
    public Dictionary<string,List<NFTsCard>> Decks;

    public List<NFTsCard> Deck;

    public List<NFTsCard> Cards;

    public List<NFTsCharacter> Characters;

    public void InitCollection()
    {
        Decks = new Dictionary<string, List<NFTsCard>>
        {
            {"Alliance", null},
            {"Spirats", null},
        };
        Cards = new List<NFTsCard>();
        Characters = new List<NFTsCharacter>();
    }

    public void ChangeDeckFaction(NFTsCharacter nFTsCharacter)
    {
        Deck = Decks[nFTsCharacter.Faction];
    }

    public void AddUnitsAndCharactersDefault()
    {
        InitCollection();
        //CHARACTERS
        Characters.Add(new NFTsCharacter()
        {
            KeyId = "Chr_1",
            Icon = "Character_1",
            Faction = "Alliance",
            CharacterId = 1
        });
        //Characters.Add(new NFTsCharacter()
        //{
        //    KeyId = "Chr_2",
        //    Name = "Dofshlu",
        //    Icon = "Character_2",
        //    Faction = "Alliance"
        //});
        //Characters.Add(new NFTsCharacter()
        //{
        //    KeyId = "Chr_3",
        //    Name = "Plagnor",
        //    Icon = "Character_3",
        //    Faction = "Alliance"
        //});
        Characters.Add(new NFTsCharacter()
        {
            KeyId = "Chr_4",
            Icon = "Character_4",
            Faction = "Spirats",
            CharacterId = 4
        });
        //ALL CARDS
        //ALLIANCE
        for (int i = 1; i <= 10; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                KeyId = $"U_ALL_{i}",
                EnergyCost = i,
                Icon = $"I_All_Ship_{i}",
                Rarity = 1,
                HitPoints = 5+(2*i),
                Shield = 3+(1*i),
                Speed = 1,
                Dammage = i,
                Faction = "Alliance"
            });
        }
        Cards.Add(new NFTsUnit()
        {
            KeyId = $"S_ALL_1",
            EnergyCost = 5,
            Icon = $"I_All_Sta_1",
            Rarity = 3,
            HitPoints = 20,
            Shield = 10,
            Speed = 0,
            Dammage = 2,
            IsStation = true,
            Faction = "Alliance"
        });
        Cards.Add(new NFTsSpell()
        {
            KeyId = $"H_NEU_1",
            EnergyCost = 10,
            Icon = $"I_Com_Skill_01",
            Rarity = 5,
            Faction = "Neutral"
        });
        //SPIRATS
        for (int i = 1; i <= 9; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                KeyId = $"U_SPI_{i}",
                EnergyCost = i,
                Icon = $"I_Spi_Ship_{i}",
                Rarity = 1,
                HitPoints = 3+(1*i),
                Shield = 5+(2*i),
                Speed = 1,
                Dammage = i,
                Faction = "Spirats"
            });
        }
        Cards.Add(new NFTsUnit()
        {
            KeyId = $"S_SPI_1",
            EnergyCost = 5,
            Icon = $"I_Spi_Sta_2",
            Rarity = 3,
            HitPoints = 5,
            Shield = 20,
            Speed = 0,
            Dammage = 1,
            IsStation = true,
            Faction = "Spirats"
        });
        //CURRENT DECK
        Decks["Alliance"] = Cards.Where(f => f.Faction == "Alliance").Take(8).ToList();
        Decks["Spirats"] = Cards.Where(f => f.Faction == "Spirats").Take(8).ToList();
        Deck = Decks["Alliance"];
    }
}
