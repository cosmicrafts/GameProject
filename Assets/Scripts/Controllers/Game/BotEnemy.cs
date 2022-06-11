using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/* 
 * This is the IA controller
 * Works with a timing loop and has 3 behaviour modes
 * Use his own energy and deck
 * The positions to deply units are predefined
 */
public class BotEnemy : MonoBehaviour
{
    //Bot IA modes
    public enum BotMode
    {
        Pasive,
        Aggressive,
        Random
    }

    //Bot player ID (always 2)
    [HideInInspector]
    public readonly int ID = 2;

    //Bot game team (always red)
    [HideInInspector]
    public readonly Team MyTeam = Team.Red;

    //Prefabs Deck units (assign in inspector)
    public GameObject[] DeckUnits = new GameObject[8];

    //IA mode (change in inspector)
    public BotMode Mode = BotMode.Pasive;

    //Cards info
    List<GameCard> GameCards;

    //Current Energy
    [Range(0, 99)]
    public float CurrentEnergy = 5;

    //Max Energy
    [Range(0, 99)]
    public float MaxEnergy = 10;

    //Energy regeneration speed
    [Range(0, 99)]
    public float SpeedEnergy = 1;

    //Delta time to make a decision
    WaitForSeconds IADelta;

    //Current own units in the game
    List<Unit> MyUnits;

    //Bot´s enemy base station
    Unit TargetUnit;

    //The cost of the most expensive card
    int MaxCostUnit;
    //The cost of the sheapest card
    int MinCostUnit;

    //Random class service
    private static System.Random rng;

    //Allows to generate energy
    bool CanGenEnergy;

    // Start is called before the first frame update
    void Start()
    {
        //Init Basic variables
        IADelta = new WaitForSeconds(2);
        MyUnits = new List<Unit>();
        CanGenEnergy = true;
        rng = new System.Random();

        //Add bot´s base station to bot's units list and set the bot´s enemy base station
        MyUnits.Add(GameMng.GM.Targets[0]);
        TargetUnit = GameMng.GM.Targets[1];

        //Check the correct size of the deck prefabs
        if (DeckUnits.Length != 8)
        {
            Debug.LogError("Size of deck must equals 8");
            DeckUnits = new GameObject[8];
        }

        //Init Deck Cards info with the units prefabs info
        GameCards = new List<GameCard>();

        for (int i = 0; i < DeckUnits.Length; i++)
        {
            if (DeckUnits[i] != null)
            {
                GameCards.Add(DeckUnits[i].GetComponent<GameCard>());
            }
        }

        if (GameCards.Count == 0)
            return;

        //Set the max and min cost of the bot´s deck
        MaxCostUnit = GameCards.Max(f => f.EnergyCost);
        MinCostUnit = GameCards.Min(f => f.EnergyCost);

        //Start IA loop
        StartCoroutine(IA());
    }

    // Update is called once per frame
    void Update()
    {
        //Generate energy if the bot's can
        if (!CanGenEnergy)
            return;
        
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += Time.deltaTime * SpeedEnergy;
        }
        else if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }

    //Set if the bot can generate energy
    public void SetCanGenEnergy(bool can)
    {
        CanGenEnergy = can;
    }

    //IA Decision algoritm
    IEnumerator IA()
    {
        //Check if the game is not ended
        while(!GameMng.GM.IsGameOver())
        {
            yield return IADelta;//Add a delay time to think
            //Check if the game is not ended and the player base station still exist
            if (TargetUnit == null || GameMng.GM.IsGameOver())
            {
                break;
            }
            //Select first unit has default to spawn
            GameCard SelectedUnit = GameCards[0];
            //Mix game cards
            GameCards.OrderBy(r => rng.Next());
            //Select a unit depending the ia mode and current energy
            switch (Mode)
            {
                case BotMode.Aggressive: //Select the most expensive card
                    {
                        if (CurrentEnergy < MaxCostUnit)
                        {
                            continue;
                        }
                        SelectedUnit = GameCards.FirstOrDefault(f => f.EnergyCost <= MaxCostUnit);
                    }
                    break;
                case BotMode.Pasive: //Select the cheapest card
                    {
                        SelectedUnit = GameCards.FirstOrDefault(f => f.EnergyCost <= MinCostUnit);
                    }
                    break;
                default: //Select a random card
                    {
                        for(int i=0; i<10; i++)
                        {
                            SelectedUnit = GameCards[Random.Range(0, GameCards.Count)];
                            if (SelectedUnit.EnergyCost <= CurrentEnergy)
                            {
                                break;
                            }
                        }
                    }
                    break;
            }

            //Check if the bot have enough energy
            if (SelectedUnit.EnergyCost <= CurrentEnergy)
            {
                //Select a random position (check the childs game objects of the bot)
                Vector3 PositionSpawn = transform.GetChild(Random.Range(0, transform.childCount)).position;
                //Spawn selected unit and rest energy
                Unit unit = GameMng.GM.CreateUnit(SelectedUnit.gameObject, PositionSpawn, MyTeam, SelectedUnit.NftsKey);
                CurrentEnergy -= SelectedUnit.EnergyCost;
            }
        }
    }
}
