using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * This is the Unit Shooter controller
 * Detects enemys and shoot bullets to attack them
 */

public class Shooter : MonoBehaviour
{
    //The enemys detector collider
    public SphereCollider EnemyDetector;

    //The unit can attack?
    [HideInInspector]
    public bool CanAttack = true;

    //Range of the enemys detector collider
    [Range(1,99)]
    public float RangeDetector = 1f;

    //Cooldown between shoots
    [Range(0, 99)]
    public float CoolDown = 1f;

    //Bullets speed
    [Range(1,99)]
    public float BulletSpeed = 10f;

    //Bullets damage
    [Range(1, 99)]
    public int BulletDamage = 1;

    //Enables if the unit must look at the enemy target
    public bool RotateToEnemy = true;

    //Enables if the unit must stop his movement if founds an enemy
    public bool StopToAttack = true;

    //The bullet game object to shoot
    public GameObject Bullet;

    //The relative position of the cannons
    public Transform Cannons;

    //The shooting particle systems
    ParticleSystem[] MuzzleFlash;

    //Current cool down for next shoot
    float DelayShoot = 0f;

    //The ship code reference
    Ship MyShip;

    //The unit code reference
    Unit MyUnit;

    //The current in range enemys
    List<Unit> InRange;

    //Current shooting target
    Unit Target;

    //Current fake shooting target
    Unit FakeTarget;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize variables
        InRange = new List<Unit>();
        EnemyDetector.radius = RangeDetector;
        MyShip = GetComponent<Ship>();
        MyUnit = GetComponent<Unit>();
        MuzzleFlash = new ParticleSystem[Cannons.childCount];
        for (int i=0; i<Cannons.childCount; i++)
        {
            MuzzleFlash[i] = Cannons.GetChild(i).GetChild(0).GetComponent<ParticleSystem>();
        }
        if (CoolDown <= 0f)
            CoolDown = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if this unit is fake
        if (MyUnit.getIsFake())
        {
            //Fake shoot (multiplayer client)
            ShootFakeTarget();
        } else
        {
            //Normal shoot
            ShootTarget();
        }
    }

    //Shoot
    public void ShootTarget()
    {
        //If the unit is death or cant attack or doesn´t has control, cancel shooting
        if (MyUnit.GetIsDeath() || !CanAttack || !MyUnit.InControl())
            return;

        //If we have a target...
        if (Target != null)
        {
            //If the unit must look at the enemy...
            if (RotateToEnemy)
            {
                //Rotate to look the enemy
                var _direction = (Target.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * 3f);
            }

            //Check if the cool down is done
            if (DelayShoot <= 0f)
            {
                //Instantiate a bullet per cannon
                for (int i = 0; i < Cannons.childCount; i++)
                {
                    Transform cannon = Cannons.GetChild(i);
                    Projectile bullet = Instantiate(Bullet, cannon.position, cannon.rotation).GetComponent<Projectile>();
                    bullet.MyTeam = MyUnit.MyTeam;
                    bullet.SetTarget(Target.gameObject);
                    bullet.Speed = BulletSpeed;
                    bullet.Dmg = BulletDamage;
                    MuzzleFlash[i].Clear();
                    MuzzleFlash[i].Play();
                }
                //Reset cooldown
                DelayShoot = CoolDown;
            }
            else //Cool down is active
            {
                //Reduce cool down
                DelayShoot -= Time.deltaTime;
            }
        }
        //If we dont have a current target
        if (Target == null)
        {
            //Check if we dont have an other enemy in range
            if (InRange.Count == 0)
            {
                //Clear the targets
                InRange.Clear();
                SetTarget(null);
            }
            else //We have more enemys in range
            {
                //Check for other enemy
                CleanEnemys();
            }
        }
    }

    //Fake Shooting
    public void ShootFakeTarget()
    {
        //If the unit is death or cant attack or doesn´t has control, cancel shooting
        if (MyUnit.GetIsDeath() || MyUnit.GetIsDisabled() && MyUnit.GetIsCasting())
            return;

        //If we have a target...
        if (FakeTarget != null)
        {
            //Check if the cool down is done
            if (DelayShoot <= 0f)
            {
                //Instantiate a fake bullet per cannon
                for (int i = 0; i < Cannons.childCount; i++)
                {
                    Transform cannon = Cannons.GetChild(i);
                    Projectile bullet = Instantiate(Bullet, cannon.position, cannon.rotation).GetComponent<Projectile>();
                    bullet.MyTeam = MyUnit.MyTeam;
                    bullet.SetLastPosition(FakeTarget.transform.position);
                    bullet.Speed = BulletSpeed;
                    bullet.Dmg = 0;
                    bullet.SetFake(true);
                    MuzzleFlash[i].Clear();
                    MuzzleFlash[i].Play();
                }
                //Reset cooldown
                DelayShoot = CoolDown;
            }
            else //Cool down is active
            {
                //Reduce cool down
                DelayShoot -= Time.deltaTime;
            }
        }
    }

    //Set the current shooting target
    public void SetTarget(Unit target)
    {
        //Set the target
        Target = target;

        //If this unit cant move or the unit must not stop when found an enemy, exit the function
        if (MyShip == null || !StopToAttack)
            return;

        //The Target is null
        if (Target == null)
        {
            //Reset the move destination of the ship
            MyShip.ResetDestination();
        } else
        {
            //Set the ship destination to the enemy
            MyShip.SetDestination(Target.transform.position, RangeDetector);
        }
    }

    //Set the fake target
    public void SetFakeTarget(Unit target)
    {
        FakeTarget = target;
    }

    //Add an enemy in the targets in range list
    public void AddEnemy(Unit enemy)
    {
        //Check if the enemy already exist in the list
        if (!InRange.Contains(enemy))
            InRange.Add(enemy);

        //If we have a target, exit the function
        if (Target != null)
            return;

        //We dont have a target, check for a target
        if (InRange.Count == 1)
            SetTarget(enemy);
        else
        {
            CleanEnemys();
        }
    }

    //Remove an enemy from the targets in range list
    public void RemoveEnemy(Unit enemy)
    {
        //Check if the enemy exist in the list
        if (InRange.Contains(enemy))
            InRange.Remove(enemy);

        //If we have a target, exit the function
        if (Target != null)
            return;

        //We dont have a target, check for a target
        if (InRange.Count == 0)
            SetTarget(null);
        else
        {
            CleanEnemys();
        }
    }

    //Clean and check for targets
    public void CleanEnemys()
    {
        InRange = InRange.Where(f => f != null).ToList();

        if (InRange.Count == 1)
        {
            SetTarget(InRange[0]);
        } else
        {
            Unit closer = InRange.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).FirstOrDefault();
            SetTarget(closer);
        }    
    }

    //Stop shooting
    public void StopAttack()
    {
        CanAttack = false;
        InRange.Clear();
        SetTarget(null);
    }

    //Get the current unit id of the target
    public int GetIdTarget()
    {
        return Target == null ? 0 : Target.getId();
    }

    //Load and initialize stats from NFT data
    public virtual void InitStatsFromNFT(NFTsUnit nFTsUnit)
    {
        if (GlobalManager.GMD.DebugMode)
            return;

        BulletDamage = nFTsUnit.Dammage;
    }
}
