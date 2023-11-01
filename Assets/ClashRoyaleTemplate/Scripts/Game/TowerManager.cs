using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerManager : GameObjectManager
{
    public override void RemoveFromGroup(Group group)
    {
        group.RemoveTower(this);
    }
}
