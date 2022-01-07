using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetClientGamePack
{
    public int GameId { get; set; }

    public List<NetUnitPack> UnitsRequested { get; set; }
}
