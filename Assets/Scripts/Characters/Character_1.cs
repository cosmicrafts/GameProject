using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_1 : GameCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        GameMng.P.SpeedEnergy = GameMng.P.SpeedEnergy * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
