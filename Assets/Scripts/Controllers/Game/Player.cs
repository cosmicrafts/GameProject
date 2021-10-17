using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public int ID = 1;

    [HideInInspector]
    public Team MyTeam = Team.Blue;

    bool InControl;

    public DragUnitCtrl UnitDrag;

    public GameObject[] DeckUnits = new GameObject[8];

    GameCard[] GameCards;
    Mesh[] UnitsMeshs;
    Material[] UnitMaterials;
    GameCard DragingCard;

    [Range(0, 99)]
    public float CurrentEnergy = 5;

    [Range(0, 99)]
    public float MaxEnergy = 10;

    [Range(0, 99)]
    public float SpeedEnergy = 1;

    private void Awake()
    {
        GameMng.P = this;
    }

    private void Start()
    {
        InControl = true;

        if (DeckUnits.Length != 8)
        {
            Debug.LogError("Size of deck must equals 8");
            DeckUnits = new GameObject[8];
        }

        GameCards = new GameCard[8];
        UnitsMeshs = new Mesh[8];
        UnitMaterials = new Material[8];

        for (int i = 0; i < DeckUnits.Length; i++)
        {
            if (DeckUnits[i] != null)
            {
                GameCards[i] = DeckUnits[i].GetComponent<GameCard>();
                UnitsMeshs[i] = GameCards[i].UnitMesh.GetComponent<MeshFilter>().sharedMesh;
                UnitMaterials[i] = GameCards[i].UnitMesh.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }

        GameMng.UI.InitGameCards(GameCards);
    }

    private void Update()
    {
        if (!InControl)
        {
            return;
        }

        AddEnergy(Time.deltaTime * SpeedEnergy);
    }

    private void FixedUpdate()
    {
        if (UnitDrag.gameObject.activeSelf && DragingCard != null)
        {
            UnitDrag.transform.position = CMath.GetMouseWorldPos();
        }
    }

    public void DragDeckUnit(int idu)
    {
        if (!InControl)
        {
            return;
        }

        DragingCard = GameCards[idu];
        UnitDrag.gameObject.SetActive(true);
        UnitDrag.SetMeshAndTexture(UnitsMeshs[idu], UnitMaterials[idu]);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
    }

    public void DropDeckUnit()
    {
        if (!InControl)
        {
            return;
        }

        if (DragingCard.EnergyCost <= CurrentEnergy && UnitDrag.IsValid())
        {
            Unit unit = Instantiate(DragingCard.gameObject, CMath.GetMouseWorldPos(), Quaternion.identity).GetComponent<Unit>();
            unit.MyTeam = MyTeam;
            RestEnergy(DragingCard.EnergyCost);
            GameMng.MT.AddDeploys(1);
        }

        UnitDrag.gameObject.SetActive(false);
        DragingCard = null;
    }

    public void SetInControl(bool incontrol)
    {
        InControl = incontrol;
        if (!InControl)
        {
            UnitDrag.gameObject.SetActive(false);
            DragingCard = null;
        }
    }

    public void AddEnergy(float value)
    {
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += value;
            GameMng.MT.AddEnergyGenerated(value);
        }
        else if (CurrentEnergy >= MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
            GameMng.MT.AddEnergyWasted(value);
        }
        GameMng.UI.UpdateEnergy(CurrentEnergy, MaxEnergy);
    }

    public void RestEnergy(float value)
    {
        CurrentEnergy -= value;
        GameMng.MT.AddEnergyUsed(value);
        GameMng.UI.UpdateEnergy(CurrentEnergy, MaxEnergy);
    }

    public bool IsDraging()
    {
        return DragingCard != null;
    }
}
