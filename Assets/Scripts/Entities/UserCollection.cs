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
            Name = "Wegnar",
            Icon = "Character_1",
            Faction = "Alliance",
            StationId = 1
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
            Name = "Sotzeer",
            Icon = "Character_4",
            Faction = "Spirats",
            StationId = 4
        });
        //ALL CARDS
        //ALLIANCE
        for (int i = 1; i <= 10; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                KeyId = $"U_ALL_{i}",
                Name = Lang.GetCardName($"U_ALL_{i}"),
                EnergyCost = 3,
                Icon = $"I_Ship_{i}",
                Rarity = 1,
                HitPoints = 10,
                Shield = 5,
                Speed = 1,
                Dammage = 3,
                Faction = "Alliance"
            });
        }
        Cards.Add(new NFTsUnit()
        {
            KeyId = $"S_ALL_1",
            Name = Lang.GetCardName($"S_ALL_1"),
            EnergyCost = 5,
            Icon = $"I_Sta_1",
            Rarity = 3,
            HitPoints = 10,
            Shield = 5,
            Speed = 0,
            Dammage = 3,
            IsStation = true,
            Faction = "Alliance"
        });
        Cards.Add(new NFTsSpell()
        {
            KeyId = $"H_NEU_1",
            Name = Lang.GetCardName($"H_NEU_1"),
            EnergyCost = 10,
            Icon = $"I_Skill_01",
            Rarity = 5,
            Faction = "Neutral"
        });
        //SPIRATS
        for (int i = 1; i <= 9; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                KeyId = $"U_SPI_{i}",
                Name = Lang.GetCardName($"U_SPI_{i}"),
                EnergyCost = 3,
                Icon = $"I_Ship_{i}",
                Rarity = 1,
                HitPoints = 10,
                Shield = 5,
                Speed = 1,
                Dammage = 3,
                Faction = "Spirats"
            });
        }
        Cards.Add(new NFTsUnit()
        {
            KeyId = $"S_SPI_1",
            Name = Lang.GetCardName($"S_SPI_1"),
            EnergyCost = 5,
            Icon = $"I_Sta_1",
            Rarity = 3,
            HitPoints = 10,
            Shield = 5,
            Speed = 0,
            Dammage = 3,
            IsStation = true,
            Faction = "Spirats"
        });
        //CURRENT DECK
        Decks["Alliance"] = Cards.Where(f => f.Faction == "Alliance").Take(8).ToList();
        Decks["Spirats"] = Cards.Where(f => f.Faction == "Spirats").Take(8).ToList();
        Deck = Decks["Alliance"];
    }
}
