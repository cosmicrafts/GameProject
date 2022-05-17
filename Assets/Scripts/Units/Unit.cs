using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Blue,
    Red
}

public enum TypeDmg
{
    Normal,
    Direct,
    Shield
}

public class Unit : MonoBehaviour
{
    protected int Id;
    protected NFTsUnit NFTs;
    protected bool IsFake;
    protected bool IsDeath;
    public int PlayerId = 1;
    public Team MyTeam;
   
    [Range(1, 9999)]
    public int HitPoints = 10;
    int MaxHp = 10;
    [Range(1, 9999)]
    public int Shield = 0;
    int MaxShield = 0;
    [Range(1, 10)]
    public float ShieldDelay = 3f;
    [Range(1, 9999)]
    public int Energy = 10;
    [Range(0, 10)]
    public float Size = 1f;
    [Range(0, 30)]
    public float SpawnAreaSize = 0f;

    [HideInInspector]
    bool IsBaseStation = false;
    [HideInInspector]
    MainStation MainStationData;

    [HideInInspector]
    public bool IsInmortal = false;

    [HideInInspector]
    protected bool Disabled = false;
    [HideInInspector]
    protected float Casting = 1f;

    float ShieldLoad = 0f;
    float ShieldCharge = 0f;
    float ShieldSpeed = 1f;

    protected SphereCollider TrigerBase;
    protected SphereCollider SolidBase;

    public GameObject Mesh;
    public GameObject SA;
    public GameObject Explosion;
    public GameObject Portal;
    public UIUnit UI;
    protected Outlinable MyOutline;
    protected Animator MyAnim;

    protected Vector3 LastImpact;

    protected Rigidbody MyRb;

    Quaternion FakeRotation;
   
    // Start is called before the first frame update
    virtual protected void Start()
    {
        MainStationData = GetComponent<MainStation>();
        IsBaseStation = MainStationData != null;
        LastImpact = Vector3.zero;
        MaxShield = Shield;
        MaxHp = HitPoints;
        MyRb = GetComponent<Rigidbody>();
        MyOutline = Mesh.GetComponent<Outlinable>();
        TrigerBase = GetComponent<SphereCollider>();
        SolidBase = Mesh.GetComponent<SphereCollider>();
        UI.Init(MaxHp - 1, MaxShield - 1);
        UI.SetColorBars(!IsMyTeam(GameMng.P.MyTeam));
        MyOutline.OutlineParameters.Color = GameMng.GM.GetColorUnit(MyTeam, PlayerId);
        TrigerBase.radius = SolidBase.radius;
        transform.localScale = new Vector3(Size, Size, Size);
        MyAnim = Mesh.GetComponent<Animator>();
        Portal.transform.parent = null;
        transform.LookAt(CMath.LookToY(transform.position, GameMng.GM.GetFinalTarget(MyTeam).position));
        SA.SetActive(IsMyTeam(GameMng.P.MyTeam) && SpawnAreaSize > 0f);
        Destroy(Portal, 3f);
        GameMng.GM.AddUnit(this);
        if (IsFake)
        {
            InitHasFake();
        }
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (Casting > 0f)
        {
            Casting -= Time.deltaTime;
            if (Casting <= 0f)
            {
                CastComplete();
            }
        }

        if (FakeRotation != null && IsFake)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, FakeRotation, Time.time * 0.1f);
        }

        if (ShieldLoad > 0f)
        {
            ShieldLoad -= Time.deltaTime;
        } else if (!IsFake)
        {
            if (Shield < MaxShield)
            {
                if (ShieldCharge < ShieldSpeed)
                {
                    ShieldCharge += Time.deltaTime;
                } else
                {
                    ShieldCharge = 0f;
                    Shield++;
                    UI.SetShieldBar((float)Shield / (float)MaxShield);
                }
            }
        }
    }

    virtual protected void FixedUpdate()
    {
        if (MyRb.velocity.magnitude > 0f)
        {
            MyRb.velocity = Vector3.zero;
        }
        if (MyRb.angularVelocity.magnitude > 0.5f)
        {
            MyRb.angularVelocity = Vector3.zero;
        }
        if (transform.rotation.x != 0f || transform.rotation.y != 0f)
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }

      //  UnitLimitControl();



    }
    void UnitLimitControl()
    {
        if (!IsBaseStation)
        {
            if (Mathf.Abs(MyRb.transform.position.z) >= 100 || Mathf.Abs(MyRb.transform.position.z) >= -100)
            {
                MyRb.velocity = Vector3.zero;
                if (MyRb.transform.position.z >= 100)
                {
                    MyRb.transform.position = new Vector3(MyRb.transform.position.x, MyRb.transform.position.x, 100);
                }
                if (MyRb.transform.position.z < -100)
                {
                    MyRb.transform.position = new Vector3(MyRb.transform.position.x, MyRb.transform.position.x, -100);
                }


            }
            if (Mathf.Abs(MyRb.transform.position.x) >= 100 || Mathf.Abs(MyRb.transform.position.x) >= -100)
            {
                MyRb.velocity = Vector3.zero;
                if (MyRb.transform.position.x >= 100)
                {
                    MyRb.transform.position = new Vector3(100, MyRb.transform.position.x, MyRb.transform.position.z);
                }
                if (MyRb.transform.position.x < -100)
                {
                    MyRb.transform.position = new Vector3(-100, MyRb.transform.position.x, MyRb.transform.position.z);
                }


            }
        }
    }
    virtual protected void CastComplete()
    {
        if (SpawnAreaSize > 0f && MyTeam == GameMng.P.MyTeam)
        {
            SA.SetActive(true);
            SA.transform.localScale = new Vector3(SpawnAreaSize, SpawnAreaSize);
        }
    }

    public void AddDmg(int dmg, TypeDmg typeDmg)
    {
        if (IsDeath || !InControl() || dmg <= 0)
            return;

        ShieldLoad = ShieldDelay;
        ShieldCharge = 0f;

        int DirectDmg = 0;

        if (Shield > 0 && typeDmg != TypeDmg.Direct)
        {
            Shield -= dmg;
            if (Shield < 0)
            {
                HitPoints += Shield;
                DirectDmg += Mathf.Abs(Shield);
                Shield = 0;
            }
            UI.SetShieldBar((float)Shield / (float)MaxShield);
        } else if (typeDmg != TypeDmg.Shield)
        {
            HitPoints -= dmg;
            DirectDmg += dmg;
        }

        if (!IsMyTeam(GameMng.P.MyTeam))
        {
            GameMng.MT.AddDamage(DirectDmg);
        }

        if (HitPoints <= 0 && !IsInmortal)
        {
            HitPoints = 0;
            Die();
        }

        UI.SetHPBar((float)HitPoints / (float)MaxHp);
    }

    public void AddDmg(int dmg)
    {
        AddDmg(dmg, TypeDmg.Normal);
    }

    public virtual void Die()
    {
        if (IsDeath)
            return;

        HitPoints = 0;
        IsDeath = true;
        UI.HideUI();
        SA.SetActive(false);
        MyAnim.SetBool("Death", true);
    }

    public virtual void DisableUnit()
    {
        Disabled = true;

        Ship ship = GetComponent<Ship>();

        if (ship != null)
            ship.CanMove = false;

        Shooter shooter = GetComponent<Shooter>();

        if (shooter != null)
            shooter.CanAttack = false;
    }

    public virtual void EnableUnit()
    {
        Disabled = false;

        Ship ship = GetComponent<Ship>();

        if (ship != null)
            ship.CanMove = true;

        Shooter shooter = GetComponent<Shooter>();

        if (shooter != null)
            shooter.CanAttack = true;
    }

    public bool IsMyTeam(Team other)
    {
        return other == MyTeam;
    }

    public bool GetIsDeath()
    {
        return IsDeath;
    }

    public bool GetIsDisabled()
    {
        return Disabled;
    }

    public bool GetIsCasting()
    {
        return Casting > 0f;
    }

    public void DestroyUnit()
    {
        GameMng.GM.DeleteUnit(this);

        GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        explosion.transform.localScale = transform.localScale * 1.8f;
        Destroy(explosion, 1f);

        if (!GameMng.GM.IsGameOver() && IsBaseStation)
        {
            GameMng.GM.EndGame(MyTeam == Team.Blue ? Team.Red : Team.Blue);
        }

        if (!IsMyTeam(GameMng.P.MyTeam))
        {
            GameMng.MT.AddKills(1);
        }

        Destroy(gameObject);
    }

    public void SetImpactPosition(Vector3 position)
    {
        LastImpact = position;
    }

    public void setId(int id)
    {
        Id = id;
    }

    public int getId()
    {
        return Id;
    }

    public string getKey()
    {
        return NFTs.KeyId;
    }

    public int GetPlayerId()
    {
        return PlayerId;
    }

    public bool getIsFake()
    {
        return IsFake;
    }

    public void setHasFake()
    {
        IsFake = true;
    }

    void InitHasFake()
    {
        TrigerBase.enabled = false;
        SolidBase.enabled = false;
    }

    public void SetFakeRotation(Quaternion quaternion)
    {
        FakeRotation = quaternion;
    }

    public Animator GetAnimator()
    {
        return MyAnim;
    }

    public int GetMaxShield()
    {
        return MaxShield;
    }

    public void SetMaxShield(int maxshield)
    {
        MaxShield = maxshield;
    }

    public void SetFakeShield(int sh, int maxshield)
    {
        if (!IsFake)
            return;

        Shield = sh;
        MaxShield = maxshield;
        UI.SetShieldBar((float)sh / (float)maxshield);
    }

    public void SetMaxHitPoints(int maxhp)
    {
        MaxHp = maxhp;
        UI.SetHPBar((float)HitPoints / (float)MaxHp);
    }

    public void SetFakeHp(int hp, int maxhp)
    {
        if (!IsFake)
            return;

        float diference = Mathf.Abs(hp - HitPoints);
        if (!IsMyTeam(GameMng.P.MyTeam))
        {
            GameMng.MT.AddDamage(diference);
        }
        HitPoints = hp;
        MaxHp = maxhp;
        UI.SetHPBar((float)hp / (float)maxhp);
    }

    public int GetMaxHitPoints()
    {
        return MaxHp;
    }

    public virtual void SetNfts(NFTsUnit nFTsUnit)
    {
        NFTs = nFTsUnit;

        if (GameData.DebugMode || nFTsUnit == null)
            return;

        HitPoints = nFTsUnit.HitPoints;
        MaxHp = HitPoints;
        Shield = nFTsUnit.Shield;
        MaxShield = Shield;

        GetComponent<Shooter>()?.InitStatsFromNFT(nFTsUnit);
    }

    public bool InControl()
    {
        return (!Disabled && Casting <= 0f && !IsFake);
    }
}
