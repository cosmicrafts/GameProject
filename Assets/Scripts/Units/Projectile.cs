using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public Team MyTeam;

    [HideInInspector]
    public GameObject Target;

    [HideInInspector]
    public float Speed;

    [HideInInspector]
    public int Dmg;

    public GameObject Inpact;
    public GameObject ShieldInpact;

    Vector3 LastTargetPosition;
    bool CheckLastPosition = false;

    private void Start()
    {
        CheckLastPosition = Target != null;
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;

        if (Target != null)
        {
            var _direction = (Target.transform.position - transform.position).normalized;
            var _lookRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * Speed);
            LastTargetPosition = Target.transform.position;
        }

        if (CheckLastPosition)
        {
            if (Vector3.Distance(transform.position, LastTargetPosition) < 1.0f)
            {
                Destroy(Instantiate(Inpact, transform.position, Quaternion.identity), 0.5f);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Target)
        {
            Unit target = Target.GetComponent<Unit>();
            Impact(target);
            return;
        }

        if (other.CompareTag("Unit"))
        {
            Unit target = other.gameObject.GetComponent<Unit>();
            if (!target.IsMyTeam(MyTeam))
            {
                Impact(target);
            }
        } else if (other.CompareTag("Out"))
        {
            Destroy(gameObject);
        }
    }

    void Impact(Unit target)
    {
        if (target.Shield > 0)
        {
            GameObject si = Instantiate(ShieldInpact, transform.position, Quaternion.identity);
            si.transform.LookAt(target.transform);
            Destroy(si, 0.5f);
        }
        else
        {
            Destroy(Instantiate(Inpact, transform.position + (transform.forward * 1.5f), Quaternion.identity), 0.5f);
        }
        target.AddDmg(Dmg);
        target.SetImpactPosition(transform.position);
        Destroy(gameObject);
    }
}
