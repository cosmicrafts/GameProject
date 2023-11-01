using UnityEngine;

public class HeroShooterManager : HeroManager
{
    [Header("Shooter:")]
    [SerializeField] private float shotVelocity = 5.0f;
    [SerializeField] private BulletManager bulletPrefab;
    [SerializeField] private Transform shotPivot;
    [SerializeField] private ParticleSystem MuzzleFlash;
    
    public override bool CanBeAsTarget(HeroManager target) { return true; }

    public override BulletManager OnFight()
    {
        BulletManager bullet = Instantiate(bulletPrefab, shotPivot.position, shotPivot.rotation);
        bullet.Shot(shotVelocity, AttackDamage, TargetObject);
        animator.Play("Attack");
        if (MuzzleFlash!= null) { MuzzleFlash.Clear(); MuzzleFlash.Play(); }
        return bullet;
    }
}