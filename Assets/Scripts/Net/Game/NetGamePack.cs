using System.Collections;
using System.Collections.Generic;

public enum NetGameStep
{
    Searching,
    Ready,
    InGame,
    Results
}

public class NetGamePack
{
    public int GameStep { get; set; }

    public string MasterPlayerName { get; set; }

    public string ClientPlayerName { get; set; }

    public string MasterWalletId { get; set; }

    public string ClientWalletId { get; set; }

    public List<NetUnitPack> Units { get; set; }
}
