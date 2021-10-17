using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public SphereCollider EnemyDetector;

    [HideInInspector]
    public bool CanAttack = true;

    [Range(1,99)]
    public float RangeDetector = 1f;

    [Range(0, 99)]
    public float CoolDown = 1f;

    [Range(1,99)]
    public float BulletSpeed = 10f;

    public bool RotateToEnemy = true;

    public bool StopToAttack = true;

    public GameObject Bullet;

    public Transform Cannons;

    ParticleSystem[] MuzzleFlash;

    float DelayShoot = 0f;

    Ship MyShip;

    Unit MyUnit;

    List<Unit> InRange;

    Unit Target;

    // Start is called before the first frame update
    void Start()
    {
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
        if (MyUnit.IsDeath() || !CanAttack || !MyUnit.InControl())
            return;

        if (RotateToEnemy && Target != null)
        {
            var _direction = (Target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * 3f);

            if (DelayShoot <= 0f)
            {
                for(int i=0; i<Cannons.childCount; i++)
                {
                    Transform cannon = Cannons.GetChild(i);
                    Projectile bullet = Instantiate(Bullet, cannon.position, cannon.rotation).GetComponent<Projectile>();
                    bullet.MyTeam = MyUnit.MyTeam;
                    bullet.Target = Target.gameObject;
                    bullet.Speed = BulletSpeed;
                    MuzzleFlash[i].Clear();
                    MuzzleFlash[i].Play();
                }
                DelayShoot = CoolDown;
            } else
            {
                DelayShoot -= Time.deltaTime;
            }
        }

        if (Target == null)
        {
            if (InRange.Count == 0)
            {
                InRange.Clear();
                SetTarget(null);
            } else
            {
                CleanEnemys();
            }
        }
    }

    public void SetTarget(Unit target)
    {
        Target = target;

        if (MyShip == null || !StopToAttack)
            return;

        if (Target == null)
        {
            MyShip.ResetDestination();
        } else
        {
            MyShip.SetDestination(Target.transform.position, RangeDetector);
        }
    }

    public void AddEnemy(Unit enemy)
    {
        if (!InRange.Contains(enemy))
            InRange.Add(enemy);

        if (Target != null)
            return;

        if (InRange.Count == 1)
            SetTarget(enemy);
        else
        {
            CleanEnemys();
        }
    }

    public void RemoveEnemy(Unit enemy)
    {
        if (InRange.Contains(enemy))
            InRange.Remove(enemy);

        if (Target != null)
            return;

        if (InRange.Count == 0)
            SetTarget(null);
        else
        {
            CleanEnemys();
        }
    }

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
}
