using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int maxplayerCard =4;

    [HideInInspector]
    public int ID = 1;
   
    [HideInInspector]
    public Team MyTeam = Team.Blue;

    bool InControl;

    bool CanGenEnergy;

    public DragUnitCtrl UnitDrag;

    public GameObject[] DeckUnits = new GameObject[4];

    GameCard[] GameCards;
    Mesh[] UnitsMeshs;
    Material[] UnitMaterials;
    GameObject[] SpellPreviews;
    GameCard DragingCard;
    GameCard SelectedCard;
    GameCharacter MyCharacter;

    [Range(0, 99)]
    public float CurrentEnergy = 5;

    [Range(0, 99)]
    public float MaxEnergy = 10;

    [Range(0, 99)]
    public float SpeedEnergy = 1;

    private void Awake()
    {
        GameMng.P = this;
        if (GameData.CurrentMatch == Match.multi)
        {
            if (!GameData.ImMaster)
            {
                ID = 2;
                MyTeam = Team.Red;
            }
        }
        GameMng.GM.InitBaseStations();
    }

    private void Start()
    {
        InControl = CanGenEnergy = true;

        if (DeckUnits.Length != maxplayerCard)
        {
            Debug.LogError("Size of deck must equals 8");
            DeckUnits = new GameObject[maxplayerCard];
            return;
        }

        //Create Character and Deck (normal game)
        if (GameData.CurrentMatch != Match.tutorial)
        {
            GameObject Character = ResourcesServices.LoadCharacterPrefab(GameMng.PlayerCharacter.KeyId);
            if (Character != null)
            {
                MyCharacter = Instantiate(Character, transform).GetComponent<GameCharacter>();
            }

            List<NFTsCard> CollectionDeck = GameMng.PlayerCollection.Deck;

            if (CollectionDeck != null)
            {
                if (CollectionDeck.Count == maxplayerCard)
                {
                    for (int i = 0; i < maxplayerCard; i++)
                    {
                        DeckUnits[i] = ResourcesServices.LoadCardPrefab(CollectionDeck[i].KeyId, CollectionDeck[i] as NFTsSpell != null);
                    }
                }
            }
        }

        GameCards = new GameCard[maxplayerCard];
        SpellPreviews = new GameObject[maxplayerCard];
        UnitsMeshs = new Mesh[maxplayerCard];
        UnitMaterials = new Material[maxplayerCard];

        for (int i = 0; i < DeckUnits.Length; i++)
        {
            if (DeckUnits[i] != null)
            {
                GameCards[i] = DeckUnits[i].GetComponent<GameCard>();
                NFTsCard nFTsCard = GameMng.PlayerCollection.Cards.FirstOrDefault(f => f.KeyId == GameCards[i].NftsKey);

                if (GameCards[i].cardType == CardType.Spell)
                {
                    SpellCard spell = GameCards[i] as SpellCard;
                    SpellPreviews[i] = spell.PreviewEffect;

                } else if (GameCards[i].cardType == CardType.Unit)
                {
                    UnitCard unit = GameCards[i] as UnitCard;
                    UnitsMeshs[i] = unit.UnitMesh.GetComponent<MeshFilter>().sharedMesh;
                    UnitMaterials[i] = unit.UnitMesh.GetComponent<MeshRenderer>().sharedMaterial;
                }   
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
       
        PlayersHotkey();

    }

    public bool ImFake()
    {
        return GameData.CurrentMatch == Match.multi && ID == 2;
    }

    public void SelectCard(int idu)
    {
        if (!InControl)
        {
            return;
        }

        if (SelectedCard == GameCards[idu])
        {
            SelectedCard = null;
            GameMng.UI.DeselectCards();
            UnitDrag.gameObject.SetActive(false);
        } else
        {
            SelectedCard = GameCards[idu];
            GameMng.UI.SelectCard(idu);
            if (SelectedCard.cardType == CardType.Spell)
            {
                PrepareDeploy(SpellPreviews[idu], SelectedCard.EnergyCost);
            }
            else if (SelectedCard.cardType == CardType.Unit)
            {
                PrepareDeploy(UnitsMeshs[idu], UnitMaterials[idu], SelectedCard.EnergyCost);
            }
        }
    }

    public void DragDeckUnit(int idu)
    {
        if (!InControl)
        {
            return;
        }

        DragingCard = GameCards[idu];

        if (SelectedCard != null && SelectedCard != DragingCard)
        {
            SelectedCard = null;
            GameMng.UI.DeselectCards();
        }

        if (DragingCard.cardType == CardType.Spell)
        {
            PrepareDeploy(SpellPreviews[idu], DragingCard.EnergyCost);
        } else if (DragingCard.cardType == CardType.Unit)
        {
            PrepareDeploy(UnitsMeshs[idu], UnitMaterials[idu], DragingCard.EnergyCost);
        }
        
    }

    public void DropDeckUnit()
    {
        if (!InControl)
        {
            return;
        }

        if (UnitDrag.IsValid() && (DragingCard != null || SelectedCard != null))
        {
            DeplyUnit(DragingCard == null ? SelectedCard : DragingCard);
        }

        UnitDrag.gameObject.SetActive(false);
        DragingCard = null;
        SelectedCard = null;
        GameMng.UI.DeselectCards();
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

    public void SetCanGenEnergy(bool can)
    {
        CanGenEnergy = can;
    }

    public void AddEnergy(float value)
    {
        if (!CanGenEnergy)
            return;

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

    public bool IsPreparingDeploy()
    {
        return DragingCard != null || SelectedCard != null;
    }

    public void PrepareDeploy(Mesh mesh, Material mat, float cost)
    {
        UnitDrag.gameObject.SetActive(true);
        UnitDrag.setMeshActive(true);
        UnitDrag.SetMeshAndTexture(mesh, mat);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }

    public void PrepareDeploy(GameObject preview, float cost)
    {
        UnitDrag.gameObject.SetActive(true);
        UnitDrag.setMeshActive(false);
        UnitDrag.CreatePreviewObj(preview);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }

    public void DeplyUnit(GameCard unitcard)
    {
        if (unitcard.EnergyCost <= CurrentEnergy)
        {
            if (unitcard as UnitCard != null) //UNIT
            {
                if (ImFake()) //Request Deploy
                {
                    Vector3 position = CMath.GetMouseWorldPos();
                    NetUnitPack unitdata = new NetUnitPack()
                    {
                        id = GameMng.GM.GenerateUnitRequestId(),
                        key = unitcard.NftsKey,
                        pos_x = position.x,
                        pos_z = position.z,
                        is_spell = false
                    };
                    GameMng.GM.RequestUnit(unitdata);
                } else //Normal Deply
                {
                    Unit unit = GameMng.GM.CreateUnit(unitcard.gameObject, CMath.GetMouseWorldPos(), MyTeam, unitcard.NftsKey);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeployUnit(unit);
                    }
                }
                RestEnergy(unitcard.EnergyCost);
                GameMng.MT.AddDeploys(1);
            } else // SPELL
            {
                if (ImFake()) //Request Deploy
                {
                    Vector3 position = CMath.GetMouseWorldPos();
                    NetUnitPack unitdata = new NetUnitPack()
                    {
                        id = GameMng.GM.GenerateUnitRequestId(),
                        key = unitcard.NftsKey,
                        pos_x = position.x,
                        pos_z = position.z,
                        is_spell = true
                    };
                    GameMng.GM.RequestUnit(unitdata);
                }
                else //Normal Deply
                {
                    Spell spell = GameMng.GM.CreateSpell(unitcard.gameObject, CMath.GetMouseWorldPos(), MyTeam, unitcard.NftsKey);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeploySpell(spell);
                    }
                }
                RestEnergy(unitcard.EnergyCost);
            }
        }
    }

    public int GetVsTeamInt()
    {
        return MyTeam == Team.Red ? 0 : 1;
    }

    public Team GetVsTeam()
    {
        return MyTeam == Team.Red ? Team.Blue : Team.Red;
    }

    public int GetVsId()
    {
        return ID == 1 ? 2 : 1;
    }

    void PlayersHotkey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DragDeckUnit(0);
            DropDeckUnit();
            SelectCard(0);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DragDeckUnit(1);
            DropDeckUnit();
            SelectCard(1);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DragDeckUnit(2);
            DropDeckUnit();
            SelectCard(2);

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DragDeckUnit(3);
            DropDeckUnit();
            SelectCard(3);

        }
    }
}
