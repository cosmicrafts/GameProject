using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class GameNetwork
{
    static NetGamePack GameNetPack;

    public static void Start()
    {
        GameNetPack = new NetGamePack();
    }

    public static int GetId()
    {
        return GameNetPack.GameId;
    }

    public static void UpdateGameData(string json)
    {
        GameNetPack = JsonConvert.DeserializeObject<NetGamePack>(json);
    }

    public static string GetJsonGameNetPack()
    {
        return JsonConvert.SerializeObject(GameNetPack);
    }

    public static void SetGameStatus(NetGameStep step)
    {
        GameNetPack.GameStep = (int)step;
    }

    public static NetGameStep GetGameStatus()
    {
        return (NetGameStep)GameNetPack.GameStep;
    }

    public static void SetGameUnits(List<NetUnitPack> netUnitPack)
    {
        GameNetPack.Units = netUnitPack;
    }

    public static void SetMasterData(string wid, string name)
    {
        GameNetPack.MasterWalletId = wid;
        GameNetPack.MasterPlayerName = name;
    }

    public static void SetClientData(string wid, string name)
    {
        GameNetPack.ClientWalletId = wid;
        GameNetPack.ClientPlayerName = name;
    }

    public static User GetVsData()
    {
        return GameData.ImMaster ? 
            new User() { NikeName = GameNetPack.ClientPlayerName, WalletId = GameNetPack.ClientWalletId } : 
            new User() { NikeName = GameNetPack.MasterPlayerName, WalletId = GameNetPack.MasterWalletId };
    }

    public static bool GameRoomIsFull()
    {
        return !string.IsNullOrEmpty(GameNetPack.ClientWalletId) && !string.IsNullOrEmpty(GameNetPack.MasterWalletId);
    }

    public static List<NetUnitPack> GetGameUnits()
    {
        return GameNetPack.Units;
    }

    [DllImport("__Internal")]
    public static extern void JSSaveScore(int score);

    [DllImport("__Internal")]
    public static extern void JSSendGameData(string json, int matchId);

    [DllImport("__Internal")]
    public static extern void JSGetGameData(int matchId);

    [DllImport("__Internal")]
    public static extern void JSCreateGame(string walletId);

    [DllImport("__Internal")]
    public static extern void JSSearchGame(string walletId);
}
