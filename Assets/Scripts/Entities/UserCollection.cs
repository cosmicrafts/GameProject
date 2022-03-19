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
    }

    public void AddUnitsAndCharactersDefault()
    {
        InitCollection();
        //CHARACTERS
        Characters.Add(new NFTsCharacter()
        {
            KeyId = "Chr_1",
            Name = "Wegnar",
            Icon = "Character_1"
        });
        Characters.Add(new NFTsCharacter()
        {
            KeyId = "Chr_2",
            Name = "Dofshlu",
            Icon = "Character_2"
        });
        Characters.Add(new NFTsCharacter()
        {
            KeyId = "Chr_3",
            Name = "Plagnor",
            Icon = "Character_3"
        });
        Characters.Add(new NFTsCharacter()
        {
            KeyId = "Chr_4",
            Name = "Sotzeer",
            Icon = "Character_4"
        });
        //ALL CARDS
        for (int i = 1; i <= 11; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                KeyId = $"Unit_{i}",
                Name = $"Unit {i}",
                EnergyCost = 3,
                Icon = $"I_Ship_{i}",
                Rarity = 1,
                HitPoints = 10,
                Shield = 5,
                Speed = 1,
                Dammage = 3
            });
        }
        for (int i = 1; i < 2; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                KeyId = $"Station_{i}",
                Name = $"Station {i}",
                EnergyCost = 5,
                Icon = $"I_Sta_{i}",
                Rarity = 3,
                HitPoints = 10,
                Shield = 5,
                Speed = 0,
                Dammage = 3,
                IsStation = true
            });
        }
        Cards.Add(new NFTsSpell()
        {
            KeyId = $"Skill_1",
            Name = $"Skill 1",
            EnergyCost = 10,
            Icon = $"I_Skill_01",
            Rarity = 5
        });
        //CURRENT DECK
        Deck = Cards.Take(8).ToList();
    }
}
