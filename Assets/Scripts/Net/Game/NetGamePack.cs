using System;
using System.Collections;
using System.Collections.Generic;

//Enum of the multiplayer status
public enum NetGameStep
{
    Waiting,
    Ready,
    InGame,
    Results,
    End
}

//Package structure for multiplayer comunication
public class NetGamePack
{
    public int GameId { get; set; }

    public int GameStep { get; set; }

    public DateTime GameStart { get; set; }

    public DateTime LastUpdate { get; set; }

    public int GameWinner { get; set; }

    public string MasterPlayerName { get; set; }

    public string ClientPlayerName { get; set; }

    public string MasterWalletId { get; set; }

    public string ClientWalletId { get; set; }

    public int MasterLvl { get; set; }

    public int ClientLvl { get; set; }

    public int MasterXp { get; set; }

    public int ClientXp { get; set; }

    public int MasterAvatar { get; set; }

    public int ClientAvatar { get; set; }

    public string MasterCharacter { get; set; }

    public string ClientCharacter { get; set; }

    public string MasterScore { get; set; }

    public string ClientScore { get; set; }

    public List<NetUnitPack> Units { get; set; }

    public List<int> DeleteIdsUnits { get; set; }
}
