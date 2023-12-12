using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class HeroManager : GameObjectManager
{
    public int Velocity { get; private set; }
    private float updtateTime;
    
    [Header("SpaceShipMovement Parameters: ")]
    [SerializeField] private int initVelocity = 2;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] public GameObject portalGameobject;
    [SerializeField] private GameObject[] propulsorsGameobject;
    private void Update()
    {
        /*foreach (GameObject propulsor in propulsorsGameobject)
        {
            propulsor.SetActive(oldPosition != newPosition);
        }*/
        
        if (TargetObject && !IsPaused && IsInitialized && isDeployReady)
        {
            transform.LookAt(TargetObject.transform, Vector3.up);
            
            Vector3 targetPosition = TargetObject.transform.position;
            Vector3 currentPosition = this.transform.position;

            float distance = Vector3.Distance(currentPosition, targetPosition);

            if(distance > minTargetDistance)
            {
                Vector3 directionOfTravel = targetPosition - currentPosition;
                directionOfTravel.Normalize();
            
                rigidbody.MovePosition(currentPosition + (directionOfTravel * Velocity * Time.deltaTime));
            }
        }
        
    }
    private void FixedUpdate()
    {
        
    }

    public override void Init(int groupIndex, SimpleVector2 initPosition, GameObjectManager initTargetObject, Material groupMaterial)
    {
        base.Init(groupIndex, initPosition, initTargetObject, groupMaterial);
        updtateTime = 0.0f;
        Velocity = initVelocity;
        portalGameobject.SetActive(true);
    }
    
    public override void RemoveFromGroup(Group group)
    {
        group.RemoveHero(this);
    }
}