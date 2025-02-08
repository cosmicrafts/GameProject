using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CosmicraftsSP
{
    public class Shooter : MonoBehaviour
    {
        public SphereCollider EnemyDetector;
        [HideInInspector]
        public bool CanAttack = true;
        [Range(1, 99)]
        public float RangeDetector = 1f;
        [Range(0, 99)]
        public float CoolDown = 1f;
        [Range(1, 99)]
        public float BulletSpeed = 10f;
        [Range(1, 99)]
        public int BulletDamage = 1;
        [Range(0f, 1f)]
        public float criticalStrikeChance = 0f;
        public float criticalStrikeMultiplier = 2.0f;
        public bool RotateToEnemy = true;
        public bool StopToAttack = true;
        public GameObject Bullet;
        public Transform[] Cannons;

        private ParticleSystem[] MuzzleFlash;
        private float DelayShoot = 0f;
        private Ship MyShip;
        private Unit MyUnit;
        private List<Unit> InRange;
        private Unit Target;

        void Start()
        {
            InRange = new List<Unit>();
            EnemyDetector.radius = RangeDetector;
            MyShip = GetComponent<Ship>();
            MyUnit = GetComponent<Unit>();
            MuzzleFlash = new ParticleSystem[Cannons.Length];
            for (int i = 0; i < Cannons.Length; i++)
            {
                if (Cannons[i].childCount > 0)
                {
                    MuzzleFlash[i] = Cannons[i].GetChild(0).GetComponent<ParticleSystem>();
                }
            }
            if (CoolDown <= 0f)
                CoolDown = 0.1f;
        }

        void Update()
        {
            ShootTarget();
        }

        public void ShootTarget()
        {
            if (MyUnit.GetIsDeath() || !CanAttack || !MyUnit.InControl())
                return;

            // Always clean enemies before checking the target.  This is the key change.
            CleanEnemies();

            if (Target != null)
            {
                // No need for a separate check here; CleanEnemies() handles validity.

                if (RotateToEnemy)
                {
                    var direction = (Target.transform.position - transform.position).normalized;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 3f);
                }

                if (DelayShoot <= 0f)
                {
                    FireProjectiles();
                    DelayShoot = CoolDown;
                    MyUnit.GetAnimator().SetTrigger("Attack");
                }
                else
                {
                    DelayShoot -= Time.deltaTime;
                }
            }
        }


        private void FireProjectiles()
        {
            for (int i = 0; i < Cannons.Length; i++)
            {
                Transform cannon = Cannons[i];

                GameObject bulletPrefab = Instantiate(Bullet, cannon.position, cannon.rotation);
                Projectile bullet = bulletPrefab.GetComponent<Projectile>();
                bullet.MyTeam = MyUnit.MyTeam;
                bullet.SetTarget(Target.gameObject);
                bullet.Speed = BulletSpeed;

                if (Random.value < criticalStrikeChance)
                {
                    bullet.Dmg = (int)(BulletDamage * criticalStrikeMultiplier);
                }
                else
                {
                    bullet.Dmg = BulletDamage;
                }

                if (MuzzleFlash[i] != null)
                {
                    MuzzleFlash[i].Clear();
                    MuzzleFlash[i].Play();
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
            }
            else
            {
                MyShip.SetDestination(Target.transform.position, RangeDetector);
            }
        }

        public void AddEnemy(Unit enemy)
        {
            if (!InRange.Contains(enemy))
                InRange.Add(enemy);

            //  No need to call CleanEnemies here. We'll do it at the start of ShootTarget.
        }

        public void RemoveEnemy(Unit enemy)
        {
            if (InRange.Contains(enemy))
                InRange.Remove(enemy);

            //  No need to call CleanEnemies here. We'll do it at the start of ShootTarget.
             if (Target == enemy) // If the removed enemy is our current target, we need to choose another one
            {
                Target = null;
            }
        }


        public void CleanEnemies()
        {
            // Remove null and dead units from InRange.
            InRange = InRange.Where(f => f != null && !f.GetIsDeath()).ToList();

            // If the target is null, dead, or no longer in range, pick a new target.
            if (Target == null || Target.GetIsDeath() || !InRange.Contains(Target))
            {
                if (InRange.Count == 0)
                {
                    SetTarget(null); // No enemies in range.
                }
                else
                {
                    // Get the closest enemy.
                    Unit closer = InRange.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).FirstOrDefault();
                    SetTarget(closer);
                }
            }
        }


        public void StopAttack()
        {
            CanAttack = false;
            InRange.Clear();
            SetTarget(null);
        }

        public int GetIdTarget()
        {
            return Target == null ? 0 : Target.getId();
        }

        public virtual void InitStatsFromNFT(NFTsUnit nFTsUnit)
        {
            if (nFTsUnit != null)
            {
                BulletDamage = nFTsUnit.Dammage;
            }
        }
    }
}