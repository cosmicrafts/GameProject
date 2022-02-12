using System;
using System.Collections.Generic;

public class NetClientGamePack
{
    public int GameId { get; set; }

    public DateTime LastUpdate { get; set; }

    public List<NetUnitPack> UnitsRequested { get; set; }
}
