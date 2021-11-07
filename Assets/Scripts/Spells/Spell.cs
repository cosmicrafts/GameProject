using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    protected int PlayerId = 1;
    public Team MyTeam;

    [Range(0, 300)]
    public float Duration = 1;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (Duration > 0)
        {
            Destroy(gameObject, Duration);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
