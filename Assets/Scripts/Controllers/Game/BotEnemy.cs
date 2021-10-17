using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotEnemy : MonoBehaviour
{
    public enum BotMode
    {
        Pasive,
        Aggressive,
        Random
    }

    [HideInInspector]
    public int ID = 2;

    [HideInInspector]
    public Team MyTeam = Team.Red;

    public GameObject[] DeckUnits = new GameObject[8];

    public BotMode Mode = BotMode.Pasive;

    List<GameCard> GameCards;

    [Range(0, 99)]
    public float CurrentEnergy = 5;

    [Range(0, 99)]
    public float MaxEnergy = 10;

    [Range(0, 99)]
    public float SpeedEnergy = 1;

    WaitForSeconds IADelta;

    List<Unit> MyUnits;

    Unit TargetUnit;

    int MaxCostUnit;
    int MinCostUnit;

    Vector3[] SideSpawns;
    int TargetSideSpawn;

    private static System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        IADelta = new WaitForSeconds(2);
        MyUnits = new List<Unit>();

        MyUnits.Add(GameMng.GM.Targets[0]);
        TargetUnit = GameMng.GM.Targets[1];

        rng = new System.Random();
        SideSpawns = new Vector3[3] { Vector3.left, Vector3.forward, Vector3.back };
        TargetSideSpawn = 0;

        if (DeckUnits.Length != 8)
        {
            Debug.LogError("Size of deck must equals 8");
            DeckUnits = new GameObject[8];
        }

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

        MaxCostUnit = GameCards.Max(f => f.EnergyCost);
        MinCostUnit = GameCards.Min(f => f.EnergyCost);

        StartCoroutine(IA());
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += Time.deltaTime * SpeedEnergy;
        }
        else if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }

    IEnumerator IA()
    {
        while(!GameMng.GM.IsGameOver())
        {
            yield return IADelta;

            if (TargetUnit == null || GameMng.GM.IsGameOver())
            {
                break;
            }

            GameCard SelectedUnit = GameCards[0];

            GameCards.OrderBy(r => rng.Next());

            switch (Mode)
            {
                case BotMode.Aggressive:
                    {
                        if (CurrentEnergy < MaxCostUnit)
                        {
                            continue;
                        }
                        SelectedUnit = GameCards.FirstOrDefault(f => f.EnergyCost <= MaxCostUnit);
                    }
                    break;
                case BotMode.Pasive:
                    {
                        SelectedUnit = GameCards.FirstOrDefault(f => f.EnergyCost <= MinCostUnit);
                    }
                    break;
                default:
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

            List<Unit> SpawnAreas = MyUnits.Where(f => f.SpawnAreaSize > 0).ToList();

            if (SpawnAreas.Count == 0)
                continue;

            Unit SpawnRefUnit = SpawnAreas.OrderBy(o => 
                Vector3.Distance(o.transform.position, TargetUnit.transform.position)).FirstOrDefault();

            if (SelectedUnit.EnergyCost <= CurrentEnergy && SpawnAreas.Count > 0)
            {
                Vector3 PositionSpawn = SpawnRefUnit.transform.position + (SideSpawns[TargetSideSpawn] * SpawnRefUnit.SpawnAreaSize);
                Unit unit = Instantiate(SelectedUnit.gameObject, PositionSpawn, Quaternion.identity).GetComponent<Unit>();
                unit.MyTeam = MyTeam;
                CurrentEnergy -= SelectedUnit.EnergyCost;
                TargetSideSpawn++;
                if (TargetSideSpawn > 2)
                    TargetSideSpawn = 0;
            }
        }
    }
}
