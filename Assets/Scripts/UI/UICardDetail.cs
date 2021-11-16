using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardDetail : UICard
{
    public GameObject Model;
    public MeshRenderer ModelRender;
    public MeshFilter ModelFilter;

    public Text Txt_Type;
    public Text Txt_Desc;
    public Text Txt_HP;
    public Text Txt_Shield;
    public Text Txt_Dmg;
    public Text Txt_Speed;

    GameObject CurrentObjPrev;

    public override void SetData(NFTsCard data)
    {
        Data = data;
        IsSelected = false;

        IsSkill = data as NFTsSpell != null;

        //Init Basic Properties
        Txt_Name.text = data.Name;
        Txt_Cost.text = data.EnergyCost.ToString();
        Txt_Rarity.text = data.Rarity.ToString();
        //Txt_Desc.text = Lang.GetText(data.KeyId);

        if (CurrentObjPrev != null)
        {
            Destroy(CurrentObjPrev);
        }

        //Types
        if (IsSkill)
        {
            //SKILLS
            SpellCard SkillPrefab = ResourcesServices.LoadCardPrefab(data.KeyId, IsSkill).GetComponent<SpellCard>();
            CurrentObjPrev = Instantiate(SkillPrefab.PreviewEffect, Model.transform.position, Quaternion.identity);
            Model.SetActive(false);
            Txt_HP.transform.parent.gameObject.SetActive(false);
            Txt_Shield.transform.parent.gameObject.SetActive(false);
            Txt_Dmg.transform.parent.gameObject.SetActive(false);
            Txt_Speed.transform.parent.gameObject.SetActive(false);
            Txt_Type.text = Lang.GetText("mn_skill");
        } else
        {
            //UNITS
            Model.SetActive(true);
            UnitCard UnitPrefab = ResourcesServices.LoadCardPrefab(data.KeyId, IsSkill).GetComponent<UnitCard>();
            ModelFilter.mesh = UnitPrefab.UnitMesh.GetComponent<MeshFilter>().sharedMesh;
            ModelRender.material = UnitPrefab.UnitMesh.GetComponent<MeshRenderer>().sharedMaterial;

            Txt_HP.transform.parent.gameObject.SetActive(true);
            Txt_Shield.transform.parent.gameObject.SetActive(true);
            Txt_Dmg.transform.parent.gameObject.SetActive(true);
            Txt_Speed.transform.parent.gameObject.SetActive(true);

            NFTsUnit unitdata = data as NFTsUnit;
            Txt_HP.text = unitdata.HitPoints.ToString();
            Txt_Shield.text = unitdata.Shield.ToString();
            Txt_Dmg.text = unitdata.Dammage.ToString();
            Txt_Speed.text = unitdata.Speed.ToString();
            Txt_Speed.transform.parent.gameObject.SetActive(!unitdata.IsStation);
            Txt_Type.text = Lang.GetText(unitdata.IsStation ? "mn_station" : "mn_ship");

        }
    }
}
