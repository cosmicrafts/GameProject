using UnityEngine;
using UnityEngine.UI;

public class UICardDetail : UICard
{
    public GameObject Model;
    public MeshRenderer ModelRender;
    public MeshFilter ModelFilter;

    public Text Txt_HP;
    public Text Txt_Shield;
    public Text Txt_Dmg;

    public Image Bar_HP;
    public Image Bar_Shield;
    public Image Bar_Dmg;

    GameObject CurrentObjPrev;

    public override void SetData(NFTsCard data)
    {
        Data = data;
        IsSelected = false;

        IsSkill = data as NFTsSpell != null;

        //Init Basic Properties
        Txt_Name.text = Lang.GetEntityName(data.KeyId);
        Txt_Details.text = Lang.GetEntityDescription(data.KeyId);
        Txt_Cost.text = data.EnergyCost.ToString();

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

            NFTsUnit unitdata = data as NFTsUnit;
            Txt_HP.text = unitdata.HitPoints.ToString();
            Bar_HP.fillAmount = (float)unitdata.HitPoints / 200f;
            Txt_Shield.text = unitdata.Shield.ToString();
            Bar_Shield.fillAmount = (float)unitdata.Shield / 200f;
            Txt_Dmg.text = unitdata.Dammage.ToString();
            Bar_Dmg.fillAmount = (float)unitdata.Dammage / 100f;
            Txt_Type.text = Lang.GetText(unitdata.IsStation ? "mn_station" : "mn_ship");
        }
    }
}
