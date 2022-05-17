using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{
    //STATIC UNIQUE SYSTEMS CONTROLLERS
    public static GameMng GM;
    public static UIGameMng UI;
    public static Player P;
    public static GameMetrics MT;
    public static Config CONFIG;
    public static User PlayerData;
    public static UserProgress PlayerProgress;
    public static UserCollection PlayerCollection;
    public static NFTsCharacter PlayerCharacter;

    //OPTIONAL SYSTEMS CONTROLLERS
    public GameTutorial GT;
    public BotEnemy BOT;

    //Code of the base stations
    [HideInInspector]
    public Unit[] Targets;
    //Base stations initial positions (set in inspector)
    public Transform[] BS_Positions;

    //Storage all game units and spells
    List<Unit> Units;
    List<Spell> Spells;
    //List of requested units for multiplayer
    List<NetUnitPack> RequestedUnits;
    //ID of the deleted and created units (necessary for multiplayer)
    List<int> DeletedUnits;
    List<int> CreatedUnits;
    //IDs auto increments for units
    int IdCounter;
    int IdRequestCounter;

    //Size of the map (set in inspector)
    public float MapWidth = 60;
    public float MapHeigth = 48;

    //Game over status
    bool GameOver = false;
    //Team winner
    [HideInInspector]
    public Team Winner;
    //Set if the time out is running
    [HideInInspector]
    public bool RunTime;

    //Time out controller
    TimeSpan TimeOut;
    //Date and time when the game begins
    DateTime StartTime;

    //clicks collider detection (necessary for spawn units)
    public BoxCollider GridColl;

    //Delta multiplayer refresh
    WaitForSeconds dnet;

    //Dictionary of NFTs data of the units <NFTkey, NFTdata> (work in progress)
    Dictionary<string, NFTsCard> AllNfts;

    private void Awake()
    {
        //Init static unique controllers
        GM = this;
        CONFIG = GameData.GetConfig();
        PlayerData = GameData.GetUserData();
        PlayerProgress = GameData.GetUserProgress();
        PlayerCollection = GameData.GetUserCollection();
        PlayerCharacter = GameData.GetUserCharacter();
        //init Basic variables and basic storages list
        GameOver = false;
        RunTime = true;
        Units = new List<Unit>();
        Spells = new List<Spell>();
        RequestedUnits = new List<NetUnitPack>();
        DeletedUnits = new List<int>();
        CreatedUnits = new List<int>();
        //init metrics controller
        MT = new GameMetrics();
        MT.InitMetrics();
        //Init NFTs dictionary
        AllNfts = new Dictionary<string, NFTsCard>();
        foreach(NFTsCard nFTsUnit in PlayerCollection.Cards)
        {
            AllNfts.Add(nFTsUnit.KeyId, nFTsUnit);
        }
        //Init Ids counters
        IdCounter = IdRequestCounter = 0;
        //Init array of base stations
        Targets = new Unit[2];
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the IDs of the base stations
        for(int i=0; i<Targets.Length; i++)
        {
            Targets[i].setId(GenerateUnitId());
        }
        //Set the time count down (5 minutes) and when the game begins
        TimeOut = new TimeSpan(0, 5, 0);
        StartTime = DateTime.Now;
        //Set the size of the clicks and taps collider (the area where player can spawn cards)
        GridColl.size = new Vector3(MapWidth*2f, 0.1f, MapHeigth*2f);
        //Check which game mode was selected
        switch(GameData.CurrentMatch)
        {
            case Match.bots: //VS IA
                {
                    //Destroy the game tutorial controller
                    Destroy(GT.gameObject);
                    //Init the Bot controller
                    BOT.gameObject.SetActive(true);
                }
                break;
            case Match.tutorial: //TUTORIAL
                {
                    //Start the game tutorial controller
                    GT.gameObject.SetActive(true);
                    //Set a specific deck to the player (from game tutorial controller)
                    P.DeckUnits = GT.DeckUnits;
                    //Stop the time count down
                    RunTime = false;
                    UI.UpdateTimeOut("--");
                    //Destroy the bot controller
                    Destroy(BOT);
                }
                break;
            case Match.multi: //MULTIPLAYER
                {
                    //Destroy the bot and tutorial controller
                    Destroy(BOT);
                    Destroy(GT.gameObject);
                    //Start the sync loop of multiplayer
                    StartCoroutine(LoopGameNetAsync());
                    //IF IM THE MASTER...
                    if (GameData.ImMaster)
                    {
                        //Set the delta time async (0.33 sec)
                        dnet = new WaitForSeconds(1f / 3f);
                        //Set and send the game status and game start datetime
                        GameNetwork.SetGameStart(DateTime.Now);
                        GameNetwork.SetGameStatus(NetGameStep.InGame);
                        SyncNetData();
                    } else //IF IM THE CLIENT
                    {
                        //Set the delta time async (5 sec)
                        dnet = new WaitForSeconds(5f);
                        //Set the base stations has fake (controlled by master data)
                        for (int i = 0; i < Targets.Length; i++)
                        {
                            Targets[i].setHasFake();
                        }
                    }
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If game is already ended, don´t do nothing
        if (GameOver)
            return;

        //Time count down controller
        TimeControl();
    }

    public void TimeControl()
    {
        //Check if the time count down is active
        if (RunTime)
        {
            //Calculate the current time remaining
            TimeSpan CurrentTime = TimeOut.Add(StartTime - DateTime.Now);
            //Update the UI
            UI.UpdateTimeOut(CurrentTime.ToString(@"m\:ss"));
            //If time out...
            if (CurrentTime.TotalSeconds <= 0)
            {
                //Stop the count down
                RunTime = false;
                //Update UI
                UI.UpdateTimeOut("0:00");
                //Player and bot can´t generate energy
                P.SetCanGenEnergy(false);
                if (GameData.CurrentMatch == Match.bots)
                {
                    BOT.SetCanGenEnergy(false);
                }
                //Check winner
                GameDraw();
            }
        }
    }

    //Check winner
    void GameDraw()
    {
        //If im the client, don´t do nothing
        if (P.ImFake())
            return;

        //Check if the base stations have the same HP
        if (Targets[0].HitPoints == Targets[1].HitPoints)
        {
            //SAME HP, so the base stations can´t atack now, only the rest of units (final fight)
            Targets[0].GetComponent<Shooter>().StopAttack();
            Targets[1].GetComponent<Shooter>().StopAttack();
        }
        else if (Targets[0].HitPoints > Targets[1].HitPoints)
        {
            //WIN P2 IF P1 BASE STATION HAS LESS HP
            KillUnit(Targets[1]);
        }
        else
        {
            //WIN P1 IF P2 BASE STATION HAS LESS HP
            KillUnit(Targets[0]);
        }
    }

    //Get the enemy base station of X team
    public Transform GetFinalTarget(Team team)
    {
        if (GameOver)
            return transform;

        return Targets[(int)team].transform;
    }

    //GAME OVER
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
            if (GameData.ImMaster)
            {
                if (GameNetwork.GetWinner() == 0)
                {
                    GameNetwork.SetWinner(winner == P.MyTeam ? 1 : 2);
                }
                GameNetwork.SetGameStatus(NetGameStep.Results);
                Debug.Log($"winer is :{winner}");
                SyncNetData();
            }
            StopCoroutine(LoopGameNetAsync());
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

    public void InitBaseStations()
    {
        //PLAYER STATION
        int PIn = P.MyTeam == Team.Blue ? 1 : 0;
        int VsIn = P.MyTeam == Team.Red ? 1 : 0;
        Targets[PIn] = Instantiate(ResourcesServices.LoadBaseStationPrefab(PlayerCharacter.Faction), 
            BS_Positions[PIn].position, Quaternion.identity).GetComponent<Unit>();
        //VS STATION
        GameObject VsStation = ResourcesServices.LoadBaseStationPrefab(
            GameData.CurrentMatch == Match.multi ? "Spirats" : "Spirats");
        Targets[VsIn] = Instantiate(VsStation, BS_Positions[VsIn].position, Quaternion.identity).GetComponent<Unit>();

        Targets[0].PlayerId = 2;
        Targets[0].MyTeam = Team.Red;
    }

    public Unit CreateUnit(GameObject obj, Vector3 position, Team team, string nftKey, int playerId = -1)
    {
        Unit unit = Instantiate(obj, position, Quaternion.identity).GetComponent<Unit>();
        unit.MyTeam = team;
        unit.PlayerId = playerId == -1 ? P.ID : playerId;
        unit.setId(GenerateUnitId());
        unit.SetNfts(AllNfts[nftKey] as NFTsUnit);
        return unit;
    }

    public Unit CreateUnit(string nftKey, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, false);
        if (obj == null)
            return null;
        return CreateUnit(obj, new Vector3(x, 0, z), (Team)team, nftKey, playerId);
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
        unit.SetNfts(null);
        unit.setHasFake();
        return unit;
    }

    public Spell CreateSpell(GameObject obj, Vector3 position, Team team, string nftKey, int playerId = -1)
    {
        Spell spell = Instantiate(obj, position, Quaternion.identity).GetComponent<Spell>();
        spell.MyTeam = team;
        spell.PlayerId = playerId == -1 ? P.ID : playerId;
        spell.setId(GenerateUnitId());
        spell.SetNfts(AllNfts[nftKey] as NFTsSpell);
        return spell;
    }

    public Spell CreateSpell(string nftKey, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, true);
        if (obj == null)
            return null;
        return CreateSpell(obj, new Vector3(x, 0, z), (Team)team, nftKey, playerId);
    }

    public Spell CreateFakeSpell(string nftKey, int id, float x, float z, int team, int playerId = -1)
    {
        GameObject obj = ResourcesServices.LoadCardPrefab(nftKey, true);
        if (obj == null)
            return null;
        Spell spell = Instantiate(obj, new Vector3(x, 0, z), Quaternion.identity).GetComponent<Spell>();
        spell.MyTeam = (Team)team;
        spell.PlayerId = playerId == -1 ? P.ID : playerId;
        spell.setId(id);
        spell.SetNfts(null);
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

    public void AddSpell(Spell spell)
    {
        if (!Spells.Contains(spell))
        {
            Spells.Add(spell);
        }
    }

    public void DeleteUnit(Unit unit)
    {
        if (Units.Contains(unit))
        {
            Units.Remove(unit);
            DeletedUnits.Add(unit.getId());
        }
    }

    public void DeleteSpell(Spell spell)
    {
        if (Spells.Contains(spell))
        {
            Spells.Remove(spell);
        }
    }

    public void KillUnit(Unit unit)
    {
        if (unit != null)
            unit.Die();
    }

    public void RequestUnit(NetUnitPack unit)
    {
        RequestedUnits.Add(unit);
        GameNetwork.SetClientLastUpdate(DateTime.Now);
        GameNetwork.SetRequestedGameUnits(RequestedUnits);
        if (GameData.IsProductionWeb())
        {
            GameNetwork.JSSendClientData(GameNetwork.GetJsonClientGameNetPack());
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
        if (GameData.CurrentMatch == Match.multi && GameData.IsProductionWeb())
        {
            GameNetwork.JSExitGame();
        }

        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }

    IEnumerator LoopGameNetAsync()
    {
        while (true)
        {
            yield return dnet;

            SyncNetData();
        }
    }

    public void SyncNetData()
    {
        if (GameData.ImMaster) //Master send main data
        {
            List<NetUnitPack> upack = Units.Select(s => new NetUnitPack
            {
                id = s.getId(),
                key = s.getKey(),
                pos_x = s.transform.position.x,
                pos_z = s.transform.position.z,
                rot_y = s.transform.rotation.eulerAngles.y,
                max_hp = s.GetMaxHitPoints(),
                max_sh = s.GetMaxShield(),
                hp = s.HitPoints,
                sh = s.Shield,
                team = (int)s.MyTeam,
                player_id = s.PlayerId,
                id_target = s.GetComponent<Shooter>() == null ? 0 : s.GetComponent<Shooter>().GetIdTarget()
            }).ToList();
            var spack = Spells.Select(s => new NetUnitPack
            {
                id = s.getId(),
                key = s.getKey(),
                pos_x = s.transform.position.x,
                pos_z = s.transform.position.z,
                rot_y = s.transform.rotation.eulerAngles.y,
                team = (int)s.MyTeam,
                player_id = s.PlayerId,
                is_spell = true
            });
            upack.AddRange(spack);
            GameNetwork.SetGameUnits(upack);
            GameNetwork.SetGameDeletedUnits(DeletedUnits);
            GameNetwork.SetMasterLastUpdate(DateTime.Now);

            try
            {
                if (GameData.IsProductionWeb())
                {
                    GameNetwork.JSSendMasterData(GameNetwork.GetJsonGameNetPack());
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        } else //Client send 
        {
            GameNetwork.SetClientLastUpdate(DateTime.Now);
            if (GameData.IsProductionWeb())
            {
                GameNetwork.JSSendClientData(GameNetwork.GetJsonClientGameNetPack());
            }
        }
    }

    public void GL_SyncMaster(string json)
    {
        if (GameData.ImMaster && !string.IsNullOrEmpty(json))
        {
            GameNetwork.UpdateClientGameData(json);
            List<NetUnitPack> UnitsRequested = GameNetwork.GetClientGameUnitsRequested();
            foreach (NetUnitPack unit in UnitsRequested)
            {
                //Create Real Unit
                if (!CreatedUnits.Contains(unit.id))
                {
                    if (unit.is_spell)
                    {
                        CreateSpell(unit.key, unit.pos_x, unit.pos_z, P.GetVsTeamInt(), P.GetVsId());
                    } else
                    {
                        CreateUnit(unit.key, unit.pos_x, unit.pos_z, P.GetVsTeamInt(), P.GetVsId());
                    }
                    CreatedUnits.Add(unit.id);
                }
            }
        }
    }

    public void GL_SyncClient(string json)
    {
        if (!GameData.ImMaster)
        {
            GameNetwork.UpdateGameData(json);
            StartTime = GameNetwork.GetStartTime();
            //Sync Real Units
            List <NetUnitPack> units = GameNetwork.GetGameUnits();
            List<int> deleted = GameNetwork.GetGameUnitsDeleted();
            foreach (NetUnitPack unit in units)
            {
                if (unit.is_spell)
                {
                    Spell find = Spells.FirstOrDefault(f => f.getId() == unit.id);
                    if (find == null) //Create fake
                    {
                        CreateFakeSpell(unit.key, unit.id, unit.pos_x, unit.pos_z, unit.team, unit.player_id);
                    }
                } else
                {
                    Unit find = Units.FirstOrDefault(f => f.getId() == unit.id);
                    if (find == null) //Create fake
                    {
                        if (!string.IsNullOrEmpty(unit.key))
                        {
                            CreateFakeUnit(unit.key, unit.id, unit.pos_x, unit.pos_z, unit.team, unit.player_id);
                        }
                    }
                    else //Sync data
                    {
                        Ship ship = find as Ship;
                        if (ship != null)
                        {
                            ship.SetFakeDestination(new Vector3(unit.pos_x, 0f, unit.pos_z));
                        }
                        find.SetFakeRotation(Quaternion.Euler(0f, unit.rot_y, 0f));
                        find.SetFakeHp(unit.hp, unit.max_hp);
                        find.SetFakeShield(unit.sh, unit.max_sh);
                        if (unit.id_target > 0)
                        {
                            Unit target = Units.FirstOrDefault(f => f.getId() == unit.id_target);
                            if (target != null)
                            {
                                Shooter shooter = find.GetComponent<Shooter>();
                                if (shooter != null)
                                {
                                    shooter.SetFakeTarget(target);
                                }
                            }
                        }
                    }
                }
            }
            //Delete Units
            foreach(int unitId in deleted)
            {
                Unit find = Units.FirstOrDefault(f => f.getId() == unitId);
                if (find != null)
                {
                    KillUnit(find);
                }
            }
            //Check winner
            CheckMultiplayerWinner();
        }
    }

    public void GL_SyncWinner(int winner)
    {
        if (!GameOver && winner != 0)
        {
            GameNetwork.SetWinner(winner);
            CheckMultiplayerWinner();
        }
    }

    public void CheckMultiplayerWinner()
    {
        if (GameData.CurrentMatch != Match.multi)
            return;

        int winner = GameNetwork.GetWinner();
        
        if (winner != 0 && !GameOver)
        {
            Debug.Log($"WE HAVE A WINNER...({winner})");
            if (winner == 1)
            {
                KillUnit(Targets[0]);
                Debug.Log("MASTER WINNS, KILLING UNIT 0");
                return;
            }
            if (winner == 2)
            {
                KillUnit(Targets[1]);
                Debug.Log("CLIENT WINNS, KILLING UNIT 1");
                return;
            }
        }
    }

    public bool MainStationsExist()
    {
        return Targets[0] != null && Targets[1] != null;
    }

    public int GenerateUnitId()
    {
        IdCounter++;
        return IdCounter;
    }

    public int GenerateUnitRequestId()
    {
        IdRequestCounter++;
        return IdRequestCounter;
    }
}
