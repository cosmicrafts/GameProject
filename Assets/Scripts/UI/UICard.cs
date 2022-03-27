using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    protected NFTsCard Data;

    public Text Txt_Name;
    public Text Txt_Cost;
    public Text Txt_Type;
    public Text Txt_Details;

    public Image Icon;

    protected bool IsSkill;

    [HideInInspector]
    public bool IsSelected;
    [HideInInspector]
    public CardClass TypeCard;
    [HideInInspector]
    public string NameCard;
    [HideInInspector]
    public string KeyName;

    public int DeckSlot;

    public virtual void SetData(NFTsCard data)
    {
        Data = data;
        IsSelected = false;

        IsSkill = data as NFTsSpell != null;
        NameCard = data.Name;
        KeyName = data.KeyId;

        Txt_Name.text = data.Name;
        Txt_Cost.text = data.EnergyCost.ToString();
        if (Txt_Details != null)
        {
            Txt_Details.text = Lang.GetCardDescription(data.KeyId);
        }
        //Txt_Rarity.text = data.Rarity.ToString();

        Icon.sprite = ResourcesServices.LoadCardIcon(data.Icon, IsSkill);

        if (data as NFTsSpell != null)
        {
            TypeCard = CardClass.Skill;
            Txt_Type.text = Lang.GetText("mn_skill");
        } else
        {
            NFTsUnit nFTsUnit = data as NFTsUnit;
            TypeCard = nFTsUnit.IsStation ? CardClass.Station : CardClass.Ship;
            Txt_Type.text = Lang.GetText(nFTsUnit.IsStation ? "mn_station" : "mn_ship");
        }
    }

    public NFTsCard GetData()
    {
        return Data;
    }

    public void SelectCard()
    {
        IsSelected = true;
        Txt_Name.color = Color.green;
    }

    public void DeselectCard()
    {
        IsSelected = false;
        Txt_Name.color = Color.white;
    }
}
