using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 * This is the Player code
 * Controls his energy, gameplay, deck, etc.
 * Contains the player data references and his ID and team on the game
 */
public class Player : MonoBehaviour
{
    //The ID of the player (or index)
    [HideInInspector]
    public int ID = 1;
    //The player´s team
    [HideInInspector]
    public Team MyTeam = Team.Blue;
    //Enables or disables the gameplay
    bool InControl;
    //Allows to generate energy
    bool CanGenEnergy;
    //Reference the drag and drop object
    [HideInInspector]
    DragUnitCtrl UnitDrag;
    //Stores the deck units prefabs
    [HideInInspector]
    public GameObject[] DeckUnits;

    //Stores the card data from units
    GameCard[] GameCards;
    //Stores the meshes and materials of the units, for drag and drop previews
    Mesh[] UnitsMeshs;
    Material[] UnitMaterials;
    //Stores the object with particules of the spells, for drag and drop previews
    GameObject[] SpellPreviews;
    //Reference the current dragin card
    GameCard DragingCard;
    //Reference the current selected card
    GameCard SelectedCard;
    //Reference the player´s character
    GameCharacter MyCharacter;

    //Curent energy
    [Range(0, 99)]
    public float CurrentEnergy = 5;
    //Max energy
    [Range(0, 99)]
    public float MaxEnergy = 10;
    //Energy regeneration speed 
    [Range(0, 99)]
    public float SpeedEnergy = 1;
    //Testing Deck
    public ShipsDataBase[] TestingDeck = new ShipsDataBase[8];
    //Hot Keys
    KeyCode[] Keys;

    private void Awake()
    {
        //Defines the current Player controler to the game manager
        GameMng.P = this;
        //Init Deck Units Array
        DeckUnits = new GameObject[8];
        //Check if the current game is a multiplayer match
        if (GlobalManager.GMD.CurrentMatch == Match.multi)
        {
            //If im not the master, im the client, so my id and team change...
            if (!GlobalManager.GMD.ImMaster)
            {
                ID = 2;
                MyTeam = Team.Red;
            }
        }
        //Game manager can now spawn the base stations
        GameMng.GM.InitBaseStations();
    }

    private void Start()
    {
        //Init key codes
        Keys = new KeyCode[8] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8};
        //Get draging cards controller
        UnitDrag = FindObjectOfType<DragUnitCtrl>();
        UnitDrag.gameObject.SetActive(false);
        //Enable the gameplay and the energy generation
        InControl = CanGenEnergy = true;

        //Check if the current match is not the tutorial
        if (GlobalManager.GMD.CurrentMatch != Match.tutorial)
        {
            //Get the player´s character and spawn it
            GameObject Character = ResourcesServices.LoadCharacterPrefab(GameMng.PlayerCharacter.KeyId);
            if (Character != null)
            {
                MyCharacter = Instantiate(Character, transform).GetComponent<GameCharacter>();
            }

            //Load Deck
            List<NFTsCard> PlayerDeck;
            //if (GameMng.GM.Testing)
            //{
            //    //Load Testing NFTs Deck
            //    PlayerDeck = new List<NFTsCard>();
            //    foreach(ShipsDataBase card in TestingDeck)
            //    {
            //        PlayerDeck.Add(new NFTsUnit()
            //        {
            //            EnergyCost = 1,
            //            IconSprite = ResourcesServices.LoadCardIcon($"U_ALL_{1}"),
            //            Rarity = 1,
            //            HitPoints = 5,
            //            Shield = 3,
            //            Speed = 1,
            //            Dammage = 1,
            //            Faction = (int)Factions.Alliance,
            //            EntType = (int)NFTClass.Ship,
            //            LocalID = 1
            //        });
            //    }
            //} else
            //{
            //    //Get the player´s collection deck
            //    PlayerDeck = GameMng.PlayerCollection.Deck;
            //}

            PlayerDeck = GameMng.PlayerCollection.Deck;

            if (PlayerDeck != null)
            {
                if (PlayerDeck.Count == 8)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        //Load unit prefabs from nft data
                        DeckUnits[i] = ResourcesServices.LoadCardPrefab(PlayerDeck[i].KeyId, PlayerDeck[i] as NFTsSpell != null);
                        //Save nft data on manager
                        GameMng.GM.AddNftCardData(PlayerDeck[i], ID);
                    }
                }
            }
        }
        //Init and set the units data arrays
        GameCards = new GameCard[8];
        SpellPreviews = new GameObject[8];
        UnitsMeshs = new Mesh[8];
        UnitMaterials = new Material[8];

        for (int i = 0; i < DeckUnits.Length; i++)
        {
            if (DeckUnits[i] != null)
            {
                GameCards[i] = DeckUnits[i].GetComponent<GameCard>();
                //NFTsCard nFTsCard = GameMng.PlayerCollection.Cards.FirstOrDefault(f => f.KeyId == GameCards[i].NftsKey);

                if ((NFTClass)GameCards[i].GetData().EntType == NFTClass.Skill)
                {
                    SpellCard spell = GameCards[i] as SpellCard;
                    SpellPreviews[i] = spell.PreviewEffect;

                } else
                {
                    UnitCard unit = GameCards[i] as UnitCard;
                    UnitsMeshs[i] = unit.UnitMesh.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
                    UnitMaterials[i] = unit.UnitMesh.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
                }   
            }
        }
        //Update the UI info
        GameMng.UI.InitGameCards(GameCards);
    }

    private void Update()
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }
        //Hot Keys
        for (int i = 0; i<8; i++)
        {
            if (Input.GetKeyDown(Keys[i]))
                DeplyUnit(GameCards[i]);
        }
        //Add energy over time
        AddEnergy(Time.deltaTime * SpeedEnergy);
        //Testing spawn random unit
        if (Input.GetKeyDown(KeyCode.R) && GameMng.GM.Testing)
        {
            string faction = Random.Range(1, 2) == 1 ? "ALL" : "SPI";
            GameMng.GM.CreateUnit($"U_{faction}_{Random.Range(1, 8)}",
                Random.Range(-30f, 30f), Random.Range(-24f, 24f), Random.Range(1, -1));
            //GameMng.UI.Players[0].gameObject.SetActive(false);
        }
    }

    //Check if im the client
    public bool ImFake()
    {
        return GlobalManager.GMD.CurrentMatch == Match.multi && ID == 2;
    }

    //Select a card (from index 0 - 7) 
    public void SelectCard(int idu)
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }

        //If the card is already selected, deselect the card
        if (SelectedCard == GameCards[idu])
        {
            SelectedCard = null;
            GameMng.UI.DeselectCards();
            UnitDrag.gameObject.SetActive(false);
        } else //Ii the card is not selected, select the card
        {
            SelectedCard = GameCards[idu];
            GameMng.UI.SelectCard(idu);
            //Show the preview of the card on the cursor
            if ((NFTClass)SelectedCard.GetData().EntType == NFTClass.Skill)
            {
                PrepareDeploy(SpellPreviews[idu], SelectedCard.GetData().EnergyCost);
            }
            else
            {
                PrepareDeploy(UnitsMeshs[idu], UnitMaterials[idu], SelectedCard.GetData().EnergyCost);
            }
        }
    }

    //Start dragin a card
    public void DragDeckUnit(int idu)
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }

        //set the dragin card
        DragingCard = GameCards[idu];
        //If the player has a selected card and is not this card, deselect the card
        if (SelectedCard != null && SelectedCard != DragingCard)
        {
            SelectedCard = null;
            GameMng.UI.DeselectCards();
        }

        //Show the preview of the card on the cursor
        if ((NFTClass)DragingCard.GetData().EntType == NFTClass.Skill)
        {
            PrepareDeploy(SpellPreviews[idu], DragingCard.GetData().EnergyCost);
        }
        {
            PrepareDeploy(UnitsMeshs[idu], UnitMaterials[idu], DragingCard.GetData().EnergyCost);
        }
        
    }

    //Drop the dragin card
    public void DropDeckUnit()
    {
        //Check if the gameplay is on
        if (!InControl)
        {
            return;
        }

        //Check if the player is dragin a card or if he has a selected card
        //Also check if the dragin card is on a valid spawn area
        if (UnitDrag.IsValid() && (DragingCard != null || SelectedCard != null))
        {
            //Deply the unit
            DeplyUnit(DragingCard == null ? SelectedCard : DragingCard);
        }
        //Clear the selection and the dragin object controller
        UnitDrag.gameObject.SetActive(false);
        DragingCard = null;
        SelectedCard = null;
        GameMng.UI.DeselectCards();
    }

    //Enables or disables the gameplay
    public void SetInControl(bool incontrol)
    {
        InControl = incontrol;
        if (!InControl)
        {
            UnitDrag.gameObject.SetActive(false);
            DragingCard = null;
        }
    }
    //Set if the player can generates energy
    public void SetCanGenEnergy(bool can)
    {
        CanGenEnergy = can;
    }
    //Add energy
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
    //Reduce energy
    public void RestEnergy(float value)
    {
        CurrentEnergy -= value;
        GameMng.MT.AddEnergyUsed(value);
        GameMng.UI.UpdateEnergy(CurrentEnergy, MaxEnergy);
    }
    //Returns if the player is dragin or selecting a card
    public bool IsPreparingDeploy()
    {
        return DragingCard != null || SelectedCard != null;
    }
    //Shows the dragin card whit the specific mesh and material
    public void PrepareDeploy(Mesh mesh, Material mat, float cost)
    {
        UnitDrag.gameObject.SetActive(true);
        UnitDrag.setMeshActive(true);
        UnitDrag.SetMeshAndTexture(mesh, mat);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }
    //Shows the dragin card whit a game object has preview
    public void PrepareDeploy(GameObject preview, float cost)
    {
        UnitDrag.gameObject.SetActive(true);
        UnitDrag.setMeshActive(false);
        UnitDrag.CreatePreviewObj(preview);
        UnitDrag.transform.position = CMath.GetMouseWorldPos();
        UnitDrag.TargetCost = cost;
    }
    //Deploy a spell or unit
    public void DeplyUnit(GameCard unitcard)
    {
        //Check the card cost and current plkayer´s energy
        if (unitcard.GetData().EnergyCost <= CurrentEnergy)
        {
            //Check if the card is a SHIP OR STATION
            if (unitcard as UnitCard != null)
            {
                //If im am the client, send a request to deploy
                if (ImFake())
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
                } else //Im not the client, so is a commun deply
                {
                    Unit unit = GameMng.GM.CreateUnit(unitcard.gameObject, CMath.GetMouseWorldPos(), MyTeam, unitcard.NftsKey);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeployUnit(unit);
                    }
                }
                //Reduce the energy
                RestEnergy(unitcard.GetData().EnergyCost);
                GameMng.MT.AddDeploys(1);
            } else //THE CARD IS A SPELL
            {
                //If im am the client, send a request to deploy
                if (ImFake())
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
                else //Im not the client, so is a commun deply
                {
                    Spell spell = GameMng.GM.CreateSpell(unitcard.gameObject, CMath.GetMouseWorldPos(), MyTeam, unitcard.NftsKey);
                    if (MyCharacter != null)
                    {
                        MyCharacter.DeploySpell(spell);
                    }
                }
                //Reduce the energy
                RestEnergy(unitcard.GetData().EnergyCost);
            }
        }
    }
    //Returns the team of the enemy as int
    public int GetVsTeamInt()
    {
        return MyTeam == Team.Red ? 0 : 1;
    }
    //Returns the team of the enemy
    public Team GetVsTeam()
    {
        return MyTeam == Team.Red ? Team.Blue : Team.Red;
    }
    //Returns the ID of the enemy
    public int GetVsId()
    {
        return ID == 1 ? 2 : 1;
    }
}
