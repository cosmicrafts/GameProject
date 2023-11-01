using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroManager : GameObjectManager
{
    public int Velocity { get; private set; }
    private Vector2 oldPosition;
    private Vector2 newPosition;
    private float updtateTime;
    private Vector3 lookAtPosition;
    
    [Header("SpaceShipMovement Parameters: ")]
    [SerializeField] private int initVelocity = 2;

    private void Update()
    {
        if (TargetObject)
        {
            updtateTime += Time.deltaTime;
            Vector2 position = Vector2.Lerp(oldPosition, newPosition, updtateTime);
            transform.position = new Vector3(position.x, 0.0f, position.y);
            transform.LookAt(lookAtPosition, Vector3.up);
        }
    }

    public override void Init(int groupIndex, SimpleVector2 initPosition, GameObjectManager initTargetObject, Material groupMaterial)
    {
        base.Init(groupIndex, initPosition, initTargetObject, groupMaterial);
        updtateTime = 0.0f;
        Velocity = initVelocity;
        oldPosition = new Vector2(initPosition.x, initPosition.z);
        newPosition = oldPosition;
    }
    
    public bool GetNewPosition(List<SimpleVector2> staticPositions, List<SimpleVector2> dynamicPositions, out SimpleVector2 position)
    {
        position = Position;
        if(initVelocity == 0) { return false; }
        if (TargetObject == null) { return false; }

        //Debug.Log(SimpleVector2.SqrDistance(Position, TargetObject.Position));
        if(SimpleVector2.SqrDistance(Position, TargetObject.Position) <= (MinTargetDistancePow ) )
        {
            return false;
        }

        SimpleVector2 delta = TargetObject.Position - Position;
        delta.ClampMagnitude(Velocity);
        delta += Position;
        SimpleVector2 newPos = delta + SimpleVector2.right;
        if (staticPositions.Contains(newPos) && !dynamicPositions.Contains(newPos))
        {
            position = newPos;
            return true;
        }
        newPos = delta + SimpleVector2.left;
        if (staticPositions.Contains(newPos) && !dynamicPositions.Contains(newPos))
        {
            position = newPos;
            return true;
        }
        newPos = delta + SimpleVector2.forward;
        if (staticPositions.Contains(newPos) && !dynamicPositions.Contains(newPos))
        {
            position = newPos;
            return true;
        }
        newPos = delta + SimpleVector2.back;
        if (staticPositions.Contains(newPos) && !dynamicPositions.Contains(newPos))
        {
            position = newPos;
            return true;
        }
        return false;
    }

    public void MoveTo(SimpleVector2 position)
    {
        updtateTime = 0.0f;
        oldPosition = newPosition;
        Position = position;
        newPosition = new Vector2(Position.x, Position.z);
    }
    public void UpdateLookAtPosition(SimpleVector2 position)
    {
        lookAtPosition = new Vector3(TargetObject.Position.x, 0.0f, TargetObject.Position.z);
        /*if((lookAtPosition - transform.position).sqrMagnitude > 5.5f)
        {
            lookAtPosition = new Vector3(position.x, 0.0f, position.z);
        }*/
    }
    
    public override void RemoveFromGroup(Group group)
    {
        group.RemoveHero(this);
    }
}