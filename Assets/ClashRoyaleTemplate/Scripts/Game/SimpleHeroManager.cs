using UnityEngine;

public class SimpleHeroManager : HeroManager
{
    [SerializeField] private Animation fightAnimation;

    public override bool CanBeAsTarget(HeroManager target)
    {
        return true;
    }
    public override BulletManager OnFight()
    {
        BulletManager bullet = gameObject.AddComponent<BulletManager>();
        bullet.Shot(5, AttackDamage, TargetObject);
        animator.Play("Attack");
       
        return bullet;
    }

    /*public override BulletManager OnFight()
    {
        //TargetObject;
        fightAnimation.Play();
        BulletManager bulletManager = new BulletManager();
        return ();
    }*/
}