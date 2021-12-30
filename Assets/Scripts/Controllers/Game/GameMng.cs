using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{
    public static GameMng GM;
    public static UIGameMng UI;
    public static Player P;
    public static GameMetrics MT;
    public static Config CONFIG;
    public static User PlayerData;
    public static UserProgress PlayerProgress;
    public static UserCollection PlayerCollection;
    public static NFTsCharacter PlayerCharacter;

    public GameTutorial GT;
    public BotEnemy BOT;

    public Unit[] Targets;

    List<Unit> Units;
    int IdCounter;

    public float MapWidth = 60;
    public float MapHeigth = 48;
    [HideInInspector]
    public float MapX = 0;
    [HideInInspector]
    public float MapZ = 0;

    bool GameOver = false;
    [HideInInspector]
    public Team Winner;
    [HideInInspector]
    public bool RunTime;

    TimeSpan TimeOut;
    DateTime StartTime;

    public BoxCollider GridColl;
    WaitForSeconds dnet;

    private void Awake()
    {
        GM = this;
        MapX = transform.position.x;
        MapZ = transform.position.z;
        GameOver = false;
        RunTime = true;
        Units = new List<Unit>();
        CONFIG = GameData.GetConfig();
        PlayerData = GameData.GetUserData();
        PlayerProgress = GameData.GetUserProgress();
        PlayerCollection = GameData.GetUserCollection();
        PlayerCharacter = GameData.GetUserCharacter();
        IdCounter = 0;
        Targets[0].PlayerId = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameOver = false;

        for(int i=0; i<Targets.Length; i++)
        {
            Targets[i].IsBaseStation = true;
        }

        TimeOut = new TimeSpan(0, 5, 0);
        StartTime = DateTime.Now;
        GridColl.size = new Vector3(MapWidth*2f, 0.1f, MapHeigth*2f);

        switch(GameData.CurrentMatch)
        {
            case Match.bots:
                {
                    Destroy(GT.gameObject);
                    BOT.gameObject.SetActive(true);
                }
                break;
            case Match.tutorial:
                {
                    GT.gameObject.SetActive(true);
                    P.DeckUnits = GT.DeckUnits;
                    RunTime = false;
                    UI.UpdateTimeOut("--");
                    Destroy(BOT);
                }
                break;
            case Match.multi:
                {
                    Destroy(BOT);
                    Destroy(GT.gameObject);
                    dnet = new WaitForSeconds(1f / 5f);
                    StartCoroutine(GameNetAsync());
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
            return;

        if (RunTime)
        {
            TimeSpan CurrentTime = TimeOut.Add(StartTime - DateTime.Now);
            UI.UpdateTimeOut(CurrentTime.ToString(@"m\:ss"));
            if (CurrentTime.TotalSeconds <= 0)
            {
                RunTime = false;
                //TIME OUT
            }
        }
    }

    public Transform GetFinalTarget(Team team)
    {
        if (GameOver)
            return transform;

        return Targets[(int)team].transform;
    }

    public void EndGame(Team winner)
    {
        GameOver = true;
        Winner = winner;
        P.SetInControl(false);
        PlayerData.FirstGame = false;

        foreach (Unit unit in Units)
        {
            unit.DisableUnit();
        }

        UI.SetGameOver(Winner);
        if (GameData.CurrentMatch == Match.multi)
        {
            //StopCoroutine(GameNetAsync());
        }
    }

    public bool IsGameOver()
    {
        return GameOver;
    }

    public int GetRemainingSecs()
    {
        return TimeOut.Add(StartTime - DateTime.Now).Seconds;
    }

    public Color GetColorUnit(Team team, int playerId)
    {
        Color result;

        if (P.MyTeam == team)
        {
            if (P.ID == playerId)
            {
                result = new Color(0f, 0.5f, 1.5f);
            }
            else
            {
                result = Color.yellow;
            }
        }
        else
        {
            result = Color.red;
        }

        return result;
    }

    public Unit CreateUnit(GameObject obj, Vector3 position, Team team, int playerId = -1)
    {
        Unit unit = Instantiate(obj, position, Quaternion.identity).GetComponent<Unit>();
        unit.MyTeam = team;
        unit.PlayerId = playerId == -1 ? P.ID : playerId;
        unit.setId(GenerateUnitId());
        return unit;
    }

    public Unit CreateFakeUnit(string nftKey, int Id, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, false);
        if (obj == null)
            return null;
        Unit unit = Instantiate(obj, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Unit>();
        unit.MyTeam = (Team)team;
        unit.PlayerId = playerId == -1 ? P.ID : playerId;
        unit.setId(Id);
        unit.setHasFake();
        return unit;
    }

    public Spell CreateSpell(GameObject obj, Vector3 position, Team team, int playerId = -1)
    {
        Spell spell = Instantiate(obj, position, Quaternion.identity).GetComponent<Spell>();
        spell.MyTeam = team;
        spell.PlayerId = playerId == -1 ? P.ID : playerId;
        return spell;
    }

    public Spell CreateFakeSpell(string nftKey, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, true);
        if (obj == null)
            return null;
        Spell spell = Instantiate(obj, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Spell>();
        spell.MyTeam = (Team)team;
        spell.PlayerId = playerId == -1 ? P.ID : playerId;
        spell.setHasFake();
        return spell;
    }

    public void AddUnit(Unit unit)
    {
        if (!Units.Contains(unit))
        {
            Units.Add(unit);
        }
    }

    public void DeleteUnit(Unit unit)
    {
        if (Units.Contains(unit))
        {
            Units.Remove(unit);
        }
    }

    public int CountUnits()
    {
        return Units.Count;
    }

    public int CountUnits(Team team)
    {
        return Units.Where(f => f.IsMyTeam(team)).Count();
    }

    public bool IsGameTutorial()
    {
        if (GT == null)
            return false;

        return GT.gameObject.activeSelf;
    }

    public void EndScene()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator GameNetAsync()
    {
        while (true)
        {
            yield return dnet;

            if (GameData.ImMaster) //Master send data
            {
                List<NetUnitPack> upack = Units.Select(s => new NetUnitPack
                {
                    id = s.getId(),
                    pos_x = s.transform.position.x,
                    pos_z = s.transform.position.z,
                    rot_y = s.transform.rotation.y,
                    max_hp = s.GetMaxHitPoints(),
                    max_sh = s.GetMaxShield(),
                    hp = s.HitPoints,
                    sh = s.Shield,
                }).ToList();
                GameNetwork.SetGameUnits(upack);

                try
                {
                    GameNetwork.JSSendGameData(GameNetwork.GetJsonGameNetPack(), GameNetwork.GetId());
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            } else //Cliente get data
            {
                
            }
        }
    }

    public void GetJson(string json)
    {
        Debug.Log(json);
    }

    public int GenerateUnitId()
    {
        IdCounter++;
        return IdCounter;
    }
}
