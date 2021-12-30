using System.Collections;
using System.Collections.Generic;

public enum NetGameStep
{
    Waiting,
    Ready,
    InGame,
    Results,
    End
}

public class NetGamePack
{
    public int GameId { get; set; }

    public int GameStep { get; set; }

    public string MasterPlayerName { get; set; }

    public string ClientPlayerName { get; set; }

    public string MasterWalletId { get; set; }

    public string ClientWalletId { get; set; }

    public int MasterLvl { get; set; }

    public int ClientLvl { get; set; }

    public int MasterAvatar { get; set; }

    public int ClientAvatar { get; set; }

    public string MasterIcon { get; set; }

    public string ClientIcon { get; set; }

    public List<NetUnitPack> Units { get; set; }

    public List<NetUnitPack> UnitsRequest { get; set; }

    public List<int> DeleteIdsUnits { get; set; }
}
