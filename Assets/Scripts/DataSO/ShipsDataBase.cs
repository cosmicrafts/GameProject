using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =("Nueva Nave") ,menuName =("Crear Nueva Nave"))]
public class ShipsDataBase : MonoBehaviour
{
    #region DataBase
    [Header("Asignar el ID")]
    [SerializeField]
    protected int id;
    [Header("Asignar el ID")]
    [SerializeField]
    protected int playerId;
    [Header("Asignar el ID")]
    [SerializeField]
    protected int hitPoints;
    [Header("Asignar el ID")]
    [SerializeField]
    protected int maxHp;
    [Header("Asignar el ID")]
    [SerializeField]
    protected int shield;
    [Header("Asignar el ID")]
    [SerializeField]
    protected int maxShield;
    [Header("Asignar el ID")]
    [SerializeField]
    protected float shieldDelay;
    [Header("Asignar el ID")]
    [SerializeField]
    protected  int energy;
    [Header("Asignar el ID")]
    [SerializeField]
    protected float size;
    [Header("Asignar el ID")]
    [SerializeField]
    protected float spawnAreaSize;
    [Header("Asignar el ID")]
    [SerializeField]
    protected float shieldLoad = 0f;
    [Header("Asignar el ID")]
    [SerializeField]
    protected float shieldCharge = 0f;
    [Header("Asignar el ID")]
    [SerializeField]
    protected float shieldSpeed = 1f;
    #endregion

    #region Variables de Lectura

    public int ID => id; //Consulta el ID
    public int PlayerID => playerId; //Consultar el Player ID
    public int HitPoins => hitPoints;//Consultar el Player hitPoints Iniciales
    public int MaxHp => maxHp;

    public int Shield => shield;

    public int MaxShield => maxShield;

    public float ShieldDelay => shieldDelay;
    public int Energy => energy;
    public float Size => size;

    public float SpawnAreaSize => spawnAreaSize;
    public float ShieldLoad => shieldLoad;
    public float ShieldCharge => shieldCharge;
    public float SshieldSpeed => shieldSpeed;
    #endregion
}
