using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
/*
* Here we save and manages the player NFTs
*/
public class UserCollection
{
    //The user collection is ready to use
    bool DeckReady;

    //All user decks
    public Dictionary<Factions, List<NFTsCard>> Decks;

    //Current selected deck
    public List<NFTsCard> Deck;

    //All user cards collection
    public List<NFTsCard> Cards;

    //All user characters
    public List<NFTsCharacter> Characters;

    //Default character (for testing)
    public NFTsCharacter DefaultCharacter;

    //Init default variables and data structures
    public void InitCollection()
    {
        DeckReady = false;
        Decks = new Dictionary<Factions, List<NFTsCard>>
        {
            {Factions.Alliance, null},
            {Factions.Spirats, null},
        };
        Cards = new List<NFTsCard>();
        Characters = new List<NFTsCharacter>();
        DefaultCharacter = new NFTsCharacter()
        {
            ID = 1,
            Name = "Wengar",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_1"),
            Faction = (int)Factions.Alliance,
            LocalID = 1,
            EntType = (int)NFTClass.Character
        };
    }

    public void ChangeDeckFaction(NFTsCharacter nFTsCharacter)
    {
        Deck = Decks[(Factions)nFTsCharacter.Faction];
    }

    public void SetCharacters(string jsonList)
    {
        Characters.AddRange(JsonConvert.DeserializeObject<List<NFTsCharacter>>(jsonList));
    }

    public void SetSpellsCards(string jsonList)
    {
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsSpell>>(jsonList));
    }

    public void SetUnitCards(string jsonList)
    {
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsUnit>>(jsonList));
    }

    //Set decks when the collection data is complete
    public void InitDecks()
    {
        //Check if the decks are already complete
        if (DeckReady || Decks == null)
            return;

        //Init Cards
        foreach (NFTsCard card in Cards)
        {
            card.TypePrefix = NFTsCollection.NFTsPrefix[card.EntType];
            card.FactionPrefix = NFTsCollection.NFTsFactionsPrefixs[(Factions)card.Faction];
        }

        //Set Factions Decks
        foreach (Factions faction in (Factions[])Enum.GetValues(typeof(Factions)))
        {
            if (faction == Factions.Neutral)
                continue;

            List<NFTsCard> factionCards = Cards.Where(f => (Factions)f.Faction == faction).ToList();

            if (factionCards.Count >= 8)
                Decks[faction] = factionCards.Take(8).ToList();
        }        

        //Set current deck
        Deck = Decks[Factions.Alliance];
        
        //Decks are redy
        DeckReady = true;
    }

    //Build a testing collection
    public void AddUnitsAndCharactersDefault()
    {
        //CHARACTERS
        Characters.Add(DefaultCharacter);
        //Characters.Add(new NFTsCharacter()
        //{
        //    Icon = "Character_4",
        //    Faction = "Spirats",
        //    LocalID = 4,
        //    EntType = (int)NFTClass.Character
        //});
        //ALL CARDS
        //ALLIANCE
        for (int i = 1; i <= 8; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                EnergyCost = i,
                IconSprite = ResourcesServices.LoadCardIcon($"U_ALL_{i}"),
                Rarity = 1,
                HitPoints = 5+(2*i),
                Shield = 3+(1*i),
                Speed = 1,
                Dammage = i,
                Faction = (int)Factions.Alliance,
                EntType = (int)NFTClass.Ship,
                LocalID = i
            });
        }
        //Cards.Add(new NFTsUnit()
        //{
        //    EnergyCost = 5,
        //    IconSprite = ResourcesServices.LoadCardIcon("S_ALL_1"),
        //    Rarity = 3,
        //    HitPoints = 20,
        //    Shield = 10,
        //    Speed = 0,
        //    Dammage = 2,
        //    EntType = (int)NFTClass.Station,
        //    Faction = (int)Factions.Alliance,
        //    LocalID = 1
        //});
        Cards.Add(new NFTsSpell()
        {
            EnergyCost = 10,
            IconSprite = ResourcesServices.LoadCardIcon("H_COM_1"),
            Rarity = 5,
            Faction = (int)Factions.Neutral,
            EntType = (int)NFTClass.Skill,
            LocalID = 1
        });
        //SPIRATS
        for (int i = 1; i <= 8; i++)
        {
            Cards.Add(new NFTsUnit()
            {
                EnergyCost = i,
                IconSprite = ResourcesServices.LoadCardIcon($"U_SPI_{i}"),
                Rarity = 1,
                HitPoints = 3+(1*i),
                Shield = 5+(2*i),
                Speed = 1,
                Dammage = i,
                Faction = (int)Factions.Spirats,
                EntType = (int)NFTClass.Ship,
                LocalID = i
            });
        }
        //Cards.Add(new NFTsUnit()
        //{
        //    EnergyCost = 5,
        //    IconSprite = ResourcesServices.LoadCardIcon($"S_SPI_1"),
        //    Rarity = 3,
        //    HitPoints = 5,
        //    Shield = 20,
        //    Speed = 0,
        //    Dammage = 1,
        //    EntType = (int)NFTClass.Station,
        //    Faction = (int)Factions.Spirats,
        //    LocalID = 1
        //});
        //CURRENT DECK
        InitDecks();
    }

    //Find a card from key
    public NFTsCard FindCard(string NFTkey)
    {
        return Cards.FirstOrDefault(f => f.KeyId == NFTkey);
    }

    //Check if player has a deck for some faction
    public bool FactionDeckExist(Factions faction)
    {
        return Decks.ContainsKey(faction);
    }
}
