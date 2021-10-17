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

    public GameTutorial GT;
    public BotEnemy BOT;

    public Unit[] Targets;

    List<Unit> Units;

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

        if (GT != null)
        {
            if (PlayerData.FirstGame)
            {
                GT.gameObject.SetActive(true);
                P.DeckUnits = GT.DeckUnits;
                RunTime = false;
                UI.UpdateTimeOut("--");
            }
            else
            {
                Destroy(GT.gameObject);
                if (BOT != null)
                {
                    BOT.gameObject.SetActive(true);
                }
            }
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
}
