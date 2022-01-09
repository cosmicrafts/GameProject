using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_01 : Spell
{
    List<Unit> Targets;
    float delaydmg;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Targets = new List<Unit>();
        delaydmg = 0.25f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 50f, 0f));
    }

    protected override void Update()
    {
        base.Update();
        
        if (IsFake)
            return;

        if (delaydmg > 0f)
        {
            delaydmg -= Time.deltaTime;
        } else
        {
            delaydmg = 0.25f;
            foreach(Unit unit in Targets)
            {
                unit.AddDmg(10, TypeDmg.Shield);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit unit = other.gameObject.GetComponent<Unit>();
            if (!Targets.Contains(unit) && !unit.IsMyTeam(MyTeam))
            {
                Targets.Add(unit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit unit = other.gameObject.GetComponent<Unit>();
            if (Targets.Contains(unit))
            {
                Targets.Remove(unit);
            }
        }
    }
}
