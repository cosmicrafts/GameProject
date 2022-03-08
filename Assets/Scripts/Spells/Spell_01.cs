using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_01 : Spell
{
    List<Unit> Targets;
    float delaydmg;
    public LineRenderer Lazer;
    public GameObject StartLazer;
    public GameObject EndLazer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Targets = new List<Unit>();
        delaydmg = 0.25f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 50f, 0f));

        if (GameMng.GM.MainStationsExist())
        {
            Vector3 target = GameMng.GM.Targets[MyTeam == Team.Blue ? 0 : 1].transform.position;
            Vector3 origin = GameMng.GM.Targets[MyTeam == Team.Blue ? 1 : 0].transform.position;
            Lazer.SetPosition(0, origin);
            Lazer.SetPosition(1, target);
            StartLazer.transform.position = origin;
            EndLazer.transform.position = target;
            StartLazer.transform.localRotation = Quaternion.Euler(new Vector3(0, MyTeam == Team.Blue ? 90 : 270, 0));
        }
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

    public override void setHasFake()
    {
        base.setHasFake();
        GetComponent<BoxCollider>().enabled = false;
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