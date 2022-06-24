using UnityEngine;

/*
 * This is the bullet code (shooted by ships or stations)
 * Controls the movement and the damage done to the target
 */

public class Projectile : MonoBehaviour
{
    //The bullet team
    [HideInInspector]
    public Team MyTeam;

    //The enemy target to reach
    GameObject Target;

    //The speed of the bullet
    [HideInInspector]
    public float Speed;

    //The damage to done on the target
    [HideInInspector]
    public int Dmg;

    //The game object effect to instantiate when the bullet impacts directly
    public GameObject Inpact;
    public Color colorImpact = new Color(1,1,1,0.5f);
    //The game object effect to instantiate when the bullet impacts on shield
    public GameObject ShieldInpact;
    public Color colorShield = new Color(1,1,1,0.5f);
    //The last valid position of the target
    Vector3 LastTargetPosition;
    //A fake bullet is used for multiplayer, to show a visual representation of the original bullet in master
    bool IsFake;

    private void Update()
    {
        //If this bullet is fake
        if (IsFake)
        {
            //Move to the target smoothly
            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Time.deltaTime * Speed);
        } else //A real bullet
        {
            //Move to the target
            transform.position += transform.forward * Time.deltaTime * Speed;
        }

        //If we have a valid target
        if (Target != null)
        {
            //Update the last target position and rotate to the target
            LastTargetPosition = Target.transform.position;
            Rotate(Target.transform.position);
        } else //If we lost the target
        {
            //Rotate to the target
            Rotate(LastTargetPosition);
            //If the bullet comes to close, destroy the bullet
            if (Vector3.Distance(transform.position, LastTargetPosition) < 1f)
            {
                transform.position = LastTargetPosition;
                GameObject impactPrefab = Instantiate(Inpact, transform.position, Quaternion.identity);
                impactPrefab.GetComponent<FX_ChangeColor>().color = colorImpact;
                impactPrefab.GetComponent<FX_ChangeColor>().UpdateColor();
                Destroy(impactPrefab, 0.5f);
                Destroy(gameObject);
            }
        }
    }

    //Collisions
    private void OnTriggerEnter(Collider other)
    {
        //If this is a fake bullet, dont do nothing
        if (IsFake)
            return;

        //Check if the other object is the bullet´s target
        if (other.gameObject == Target)
        {
            //Impact
            Unit target = Target.GetComponent<Unit>();
            Impact(target);
            return;
        }

        //If the other object is a unit...
        if (other.CompareTag("Unit"))
        {
            //And is an enemy unit...
            Unit target = other.gameObject.GetComponent<Unit>();
            if (!target.IsMyTeam(MyTeam))
            {
                //Impact
                Impact(target);
            }
        } else if (other.CompareTag("Out")) //If the bullets go out of the map...
        {
            //Destroy it
            Destroy(gameObject);
        }
    }

    //Target Impact
    void Impact(Unit target)
    {
        if (target.Shield > 0) //Check if the target has shield
        {
            //Instantiate the shield impact
            GameObject si = Instantiate(ShieldInpact, transform.position, Quaternion.identity);
            si.GetComponent<FX_ChangeColor>().color = colorShield;
            si.GetComponent<FX_ChangeColor>().UpdateColor();
            si.transform.LookAt(target.transform);
            Destroy(si, 0.5f);
        }
        else
        {
            //Instantiate the direct impact
            GameObject impactPrefab = Instantiate(Inpact, transform.position + (transform.forward * 1.5f), Quaternion.identity);
            impactPrefab.GetComponent<FX_ChangeColor>().color = colorImpact;
            impactPrefab.GetComponent<FX_ChangeColor>().UpdateColor();
            Destroy(impactPrefab, 0.5f);

        }
        //Add damage to the target
        target.AddDmg(Dmg);
        target.SetImpactPosition(transform.position);
        //Destroy the bullet
        Destroy(gameObject);
    }

    //Rotate the bullet looking to the target
    void Rotate(Vector3 target)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((target - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * Speed);
    }

    //Set the last target position
    public void SetLastPosition(Vector3 lastposition)
    {
        LastTargetPosition = lastposition;
    }

    //Set the current enemy target
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

    //Set the bullet as fake
    public void SetFake(bool isfake)
    {
        IsFake = isfake;
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null)
            sc.enabled = !isfake;
    }
}
