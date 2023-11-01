using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GameObjectManager : MonoBehaviour
{
    public SimpleVector2 Position { get; protected set; }
    public int HitPoints { get; private set; }
    public int AttackDamage { get; private set; }
    public int MinTargetDistancePow { get; private set; }
    
    public GameObjectManager TargetObject { get; protected set; }
    public bool IsBomb { get; protected set; }
    
    [Range(1, 9999)] [SerializeField] protected int MaxHitPoints = 500;
    [Range(1, 99)]   [SerializeField] protected int startAttackDamage = 50;
    [Range(1, 25)]   [SerializeField] protected int minTargetDistance = 1;
    [SerializeField] private UIUnit_Multi UIUnit;
    [SerializeField] private bool isBomb = false;
    [SerializeField] protected Animator animator;
    [SerializeField] private GameObject explosionFX;
    [SerializeField] public GameObject previewMeshObject;
    [SerializeField] public Material transparentMaterial;
    
    public virtual void Init(int groupIndex, SimpleVector2 initPosition, GameObjectManager initTargetObject, Material groupMaterial)
    {
        HitPoints = MaxHitPoints;
        AttackDamage = startAttackDamage;
        Position = initPosition;
        TargetObject = initTargetObject;
        transform.position = new Vector3(initPosition.x, 0.0f, initPosition.z);
        if (UIUnit)
        {
            UIUnit.SetColorBars(groupMaterial.color);
        }
        MinTargetDistancePow = minTargetDistance * minTargetDistance;
        IsBomb = isBomb;
    }
    public void SetTargetObject(GameObjectManager targetObject)
    {
        TargetObject = targetObject;
    }
    public virtual bool Damage(int force)
    {
        if(MaxHitPoints == 0)
        {
            return false;
        }
        HitPoints -= force;
        if (HitPoints > 0)
        {
            UIUnit.SetHPBar((float)HitPoints / (float)MaxHitPoints);
        }
        else
        {
            UIUnit.SetHPBar(0);
        }
        return HitPoints <= 0;
    }
    public virtual void Remove()
    {
        animator.Play("Explosion");
        GameObject explosionObject = Instantiate(explosionFX, transform.position, transform.rotation);
        Destroy(explosionObject, 2);
        Destroy(gameObject, 1);
        
    }

    public void RemoveTargetObject()
    {
        if (TargetObject)
        {
            TargetObject.Remove();
            TargetObject = null;
        }
    }
    
    public bool CanShoot_IsInRange()
    {
        if (TargetObject && GetType() == typeof(HeroBombManager))
        {
            Debug.Log((Position - TargetObject.Position).SqrMagnitude +  "  " + MinTargetDistancePow);
        }
        
        return TargetObject && (Position - TargetObject.Position).SqrMagnitude < MinTargetDistancePow + 1;
    }
    public abstract BulletManager OnFight();
    public abstract bool CanBeAsTarget(HeroManager target);
    public abstract void RemoveFromGroup(Group group);
}
