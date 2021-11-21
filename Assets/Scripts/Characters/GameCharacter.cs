using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    public string IdKey;

    public Sprite Icon;

    public virtual void DeployUnit(Unit unit)
    {

    }

    public virtual void DeploySpell(Spell spell)
    {

    }
}
