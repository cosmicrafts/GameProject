using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public Team MyTeam;

    GameObject Target;

    [HideInInspector]
    public float Speed;

    [HideInInspector]
    public int Dmg;

    public GameObject Inpact;
    public GameObject ShieldInpact;

    Vector3 LastTargetPosition;
    bool IsFake;

    private void Start()
    {

    }

    private void Update()
    {
        if (IsFake)
        {
            //transform.position = Vector3.Lerp(transform.position, LastTargetPosition, Time.deltaTime * 1f/(Speed*0.1f));
            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Time.deltaTime * Speed);
        } else
        {
            transform.position += transform.forward * Time.deltaTime * Speed;
        }

        if (Target != null)
        {
            LastTargetPosition = Target.transform.position;
            Rotate(Target.transform.position);
        } else
        {
            Rotate(LastTargetPosition);
            if (Vector3.Distance(transform.position, LastTargetPosition) < 1f)
            {
                transform.position = LastTargetPosition;
                Destroy(Instantiate(Inpact, transform.position, Quaternion.identity), 0.5f);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsFake)
            return;

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

    void Rotate(Vector3 target)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((target - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * Speed);
    }

    public void SetLastPosition(Vector3 lastposition)
    {
        LastTargetPosition = lastposition;
    }

    public void SetTarget(GameObject target)
    {
        if (target == null)
        {
            Destroy(gameObject, 1f);
        } else
        {
            Target = target;
            LastTargetPosition = target.transform.position;
        }
    }

    public void SetFake(bool isfake)
    {
        IsFake = isfake;
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null)
            sc.enabled = !isfake;
    }
}
