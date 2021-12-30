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

    public static void SetGameDeletedUnits(List<int> netUnitsDeleted)
    {
        GameNetPack.DeleteIdsUnits = netUnitsDeleted;
    }

    public static void SetMasterData(string wid, string name)
    {
        GameNetPack.MasterWalletId = wid;
        GameNetPack.MasterPlayerName = name;
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
                Level = GameNetPack.ClientLvl,
                Avatar = GameNetPack.ClientAvatar,
                Icon = GameNetPack.ClientIcon
            } : 
            new UserGeneral() { 
                NikeName = GameNetPack.MasterPlayerName, 
                WalletId = GameNetPack.MasterWalletId,
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

    [DllImport("__Internal")]
    public static extern void JSSaveScore(int score);

    [DllImport("__Internal")]
    public static extern void JSSendGameData(string json, int matchId);

    [DllImport("__Internal")]
    public static extern void JSCreateUnitRequest(string json, int matchId);

    //[DllImport("__Internal")]
    //public static extern void JSGetGameData(int matchId);

    //[DllImport("__Internal")]
    //public static extern void JSCreateGame(string walletId, string playerData);

    [DllImport("__Internal")]
    public static extern void JSSearchGame(string walletId, string playerData);
}
