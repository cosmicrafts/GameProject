using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =("Nueva Nave") ,menuName =("Crear Nueva Nave"))]
public class ShipsDataBase : ScriptableObject
{

    #region DataBase

    //----Ship´s name-----------------------------------------------------------
    [Tooltip("Informa de la Nombre de la carta")]
    [Header("Nombre de la unidad")]
    [SerializeField]
    protected string Name;

    //----Nft local ID-----------------------------------------------------------
    [Tooltip("Informa el HP de la carta")]
    [Header("Asignar el HitPoint de la carta")]
    [SerializeField]
    protected int LocalID;

    //----Nft faction-----------------------------------------------------------
    [Tooltip("Informa el HP de la carta")]
    [Header("Asignar el HitPoint de la carta")]
    [SerializeField]
    protected Factions Faction;

    //----Nft card type-----------------------------------------------------------
    [Tooltip("Informa el HP de la carta")]
    [Header("Asignar el HitPoint de la carta")]
    [SerializeField]
    protected NFTClass NftType;

    //----Unit HP-----------------------------------------------------------
    [Tooltip("Informa el HP de la carta")]
    [Header("Asignar el HitPoint de la carta")]
    [Range(1, 9999)]
    [SerializeField]
    protected int HitPoints;

    //----Unit Shield-----------------------------------------------------------
    [Tooltip("Informa el Escudo de la carta")]
    [Header("Asignar el Escudo de la carta")]
    [SerializeField]
    [Range(1, 9999)]
    protected int Shield;

    //----Unit card cost-----------------------------------------------------------
    [Tooltip("Costo de energia de la unidad")]
    [Header("Asignar el costo de la unidad")]
    [SerializeField]
    [Range(1, 9999)]
    protected int EnergyCost;

    //----Unit Damage-----------------------------------------------------------
    [Tooltip("Asignar la tamaño para el escudo")]
    [Header("Asignar el tamaño para la nave")]
    [SerializeField]
    [Range(0, 9999)]
    protected int Dammage;

    //----Unit Speed-----------------------------------------------------------
    [Tooltip("Asignar la area de buff para el zona invocion")]
    [Header("Asignar el tamaño para el buff de la zona de invocacion")]
    [SerializeField]
    [Range(0, 99)]
    protected float Speed;

    #endregion

    #region Variables de Lectura

    #endregion
}
