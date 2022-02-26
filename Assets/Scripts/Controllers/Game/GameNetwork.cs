using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class GameNetwork
{
    static NetGamePack GameNetPack;
    static NetClientGamePack ClientNetPack;

    public static void Start()
    {
        GameNetPack = new NetGamePack();
        ClientNetPack = new NetClientGamePack();
        GameNetPack.LastUpdate = DateTime.Now;
        ClientNetPack.LastUpdate = DateTime.Now;
    }

    public static int GetId()
    {
        return GameNetPack.GameId;
    }

    public static void UpdateGameData(string json)
    {
        GameNetPack = JsonConvert.DeserializeObject<NetGamePack>(json);
    }

    public static void UpdateClientGameData(string json)
    {
        ClientNetPack = JsonConvert.DeserializeObject<NetClientGamePack>(json);
    }

    public static string GetJsonGameNetPack()
    {
        return JsonConvert.SerializeObject(GameNetPack);
    }

    public static string GetJsonClientGameNetPack()
    {
        return JsonConvert.SerializeObject(ClientNetPack);
    }

    public static void SetGameStatus(NetGameStep step)
    {
        GameNetPack.GameStep = (int)step;
    }

    public static void SetGameStart(DateTime start)
    {
        GameNetPack.GameStart = start;
    }

    public static void SetClientLastUpdate(DateTime dateTime)
    {
        ClientNetPack.LastUpdate = dateTime;
    }

    public static void SetMasterLastUpdate(DateTime dateTime)
    {
        GameNetPack.LastUpdate = dateTime;
    }

    public static void SetWinner(int GameWinner)
    {
        GameNetPack.GameWinner = GameWinner;
    }

    public static int GetWinner()
    {
        return GameNetPack.GameWinner;
    }

    public static NetGameStep GetGameStatus()
    {
        return (NetGameStep)GameNetPack.GameStep;
    }

    public static void SetGameUnits(List<NetUnitPack> netUnitPack)
    {
        GameNetPack.Units = netUnitPack;
    }

    public static void SetGameDeletedUnits(List<int> netUnitsDeleted)
    {
        GameNetPack.DeleteIdsUnits = netUnitsDeleted;
    }

    public static void SetRequestedGameUnits(List<NetUnitPack> netUnitPack)
    {
        ClientNetPack.UnitsRequested = netUnitPack;
    }

    public static void SetClientGameId(int gameId)
    {
        ClientNetPack.GameId = gameId;
    }

    public static void SetMasterData(string wid, string name)
    {
        GameNetPack.MasterWalletId = wid;
        GameNetPack.MasterPlayerName = name;
    }

    public static DateTime GetStartTime()
    {
        return GameNetPack.GameStart;
    }

    public static string GetMasterWalletId()
    {
        return GameNetPack.MasterWalletId;
    }

    public static void SetClientData(string wid, string name)
    {
        GameNetPack.ClientWalletId = wid;
        GameNetPack.ClientPlayerName = name;
    }

    public static string GetClientWalletId()
    {
        return GameNetPack.ClientWalletId;
    }

    public static UserGeneral GetVsData()
    {
        return GameData.ImMaster ? 
            new UserGeneral() { 
                NikeName = GameNetPack.ClientPlayerName, 
                WalletId = GameNetPack.ClientWalletId,
                Xp = GameNetPack.ClientXp,
                Level = GameNetPack.ClientLvl,
                Avatar = GameNetPack.ClientAvatar,
                Icon = GameNetPack.ClientIcon
            } : 
            new UserGeneral() { 
                NikeName = GameNetPack.MasterPlayerName, 
                WalletId = GameNetPack.MasterWalletId,
                Xp = GameNetPack.MasterXp,
                Level = GameNetPack.MasterLvl,
                Avatar = GameNetPack.MasterAvatar,
                Icon = GameNetPack.MasterIcon
            };
    }

    public static bool GameRoomIsFull()
    {
        return !string.IsNullOrEmpty(GameNetPack.ClientWalletId) && !string.IsNullOrEmpty(GameNetPack.MasterWalletId);
    }

    public static List<NetUnitPack> GetGameUnits()
    {
        return GameNetPack.Units;
    }

    public static List<int> GetGameUnitsDeleted()
    {
        return GameNetPack.DeleteIdsUnits;
    }

    public static List<NetUnitPack> GetClientGameUnitsRequested()
    {
        return ClientNetPack.UnitsRequested;
    }

    [DllImport("__Internal")]
    public static extern void JSSaveScore(int score);

    [DllImport("__Internal")]
    public static extern void JSSendMasterData(string json);

    [DllImport("__Internal")]
    public static extern void JSSendClientData(string json);

    [DllImport("__Internal")]
    public static extern void JSExitGame();

    [DllImport("__Internal")]
    public static extern void JSSearchGame(string json);
}
