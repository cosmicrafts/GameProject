﻿using SensorToolkit;
using UnityEngine;

public class Ship : Unit
{
    public SteeringRig MySt;

    float Speed = 0f;

    [Range(0,99)]
    public float Aceleration = 1f;
    [Range(0,99)]
    public float MaxSpeed = 10f;
    [Range(0,99)]
    public float DragSpeed = 1f;
    [Range(0, 99)]
    public float TurnSpeed = 5f;
    [Range(0, 99)]
    public float StopSpeed = 5f;
    [Range(0, 10)]
    public float StoppingDistance = 0.5f;
    [Range(0, 50)]
    public float AvoidanceRange = 3f;

    Transform Target;
    Vector3 FakeDestination;
    public RaySensor[] AvoidanceSensors;
    public GameObject MainThruster;

    [HideInInspector]
    public bool CanMove = true;

    Vector3 DeathRot;

    protected override void Start()
    {
        base.Start();
        Target = GameMng.GM.GetFinalTarget(MyTeam);
        FakeDestination = transform.position;
        if (IsFake)
        {
            MySt.enabled = false;
        } else
        {
            MySt.Destination = Target.position;
            MySt.StoppingDistance = StoppingDistance;
            foreach (RaySensor sensor in AvoidanceSensors)
            {
                sensor.Length = AvoidanceRange;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void FixedUpdate()
    {
        if (MyRb.linearVelocity.magnitude > MaxSpeed+1f)
        {
            MyRb.linearVelocity = MyRb.linearVelocity.normalized * (MaxSpeed + 1f);
        }
        if (MyRb.angularVelocity.magnitude > 0.5f)
        {
            MyRb.angularVelocity = Vector3.zero;
        }
        if (transform.rotation.x != 0f || transform.rotation.y != 0f)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    void Move()
    {
        if (IsDeath)
        {
            transform.Rotate(DeathRot, 100f * Time.deltaTime, Space.Self);
            return;
        }

        if (IsFake)
        {
            transform.position = Vector3.Lerp(transform.position, FakeDestination, Time.deltaTime * MaxSpeed);
            MainThruster.SetActive(Vector3.Distance(transform.position, FakeDestination) > Speed);
        } else
        {
            if (InControl())
            {
                if (CanMove)
                {
                    if (Speed < MaxSpeed)
                    {
                        Speed += Aceleration * Time.deltaTime;
                    }
                    else
                    {
                        Speed = MaxSpeed;
                    }

                    MySt.TurnForce = TurnSpeed * 100f;
                    MySt.StrafeForce = DragSpeed * 100f;
                    MySt.MoveForce = Speed * 100f;
                    MySt.StopSpeed = StopSpeed;
                }
                else
                {
                    MySt.TurnForce = 0f;
                    MySt.MoveForce = 0f;
                    Speed = 0f;
                }

                if (MySt.hasReachedDestination() && MainThruster.activeSelf)
                {
                    MainThruster.SetActive(false);
                }
                if (!MySt.hasReachedDestination() && !MainThruster.activeSelf)
                {
                    MainThruster.SetActive(true);
                }
            }
            else if (MainThruster.activeSelf)
            {
                MainThruster.SetActive(false);
                MySt.TurnForce = 0f;
                MySt.MoveForce = 0f;
                Speed = 0f;
            }
        }
    }

    public void ResetDestination()
    {
        if (!InControl())
            return;

        MySt.Destination = Target.position;
        MySt.StoppingDistance = StoppingDistance;
    }

    public void SetDestination(Vector3 des, float stopdistance)
    {
        MySt.Destination = des;
        MySt.StoppingDistance = stopdistance;
    }

    public void SetFakeDestination(Vector3 des)
    {
        FakeDestination = des;
    }

    protected override void CastComplete()
    {
        base.CastComplete();
    }

    public override void Die()
    {
        base.Die();
        MySt.enabled = false;
        MainThruster.SetActive(false);
        float AngleDeathRot = CMath.AngleBetweenVector2(LastImpact, transform.position);

        float z = Mathf.Sin(AngleDeathRot * Mathf.Deg2Rad);
        float x = Mathf.Cos(AngleDeathRot * Mathf.Deg2Rad);
        DeathRot = new Vector3(x, 0, z);
    }

    public override void DisableUnit()
    {
        base.DisableUnit();
    }

    public override void EnableUnit()
    {
        base.EnableUnit();
    }

    public override void SetNfts(NFTsUnit nFTsUnit)
    {
        base.SetNfts(nFTsUnit);

        if (GameData.DebugMode || nFTsUnit == null)
            return;

        MaxSpeed = nFTsUnit.Speed;
    }
}
