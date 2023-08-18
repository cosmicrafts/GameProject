using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UICharacters : MonoBehaviour
{
    //Player NFT collection reference
    public UserCollection PlayerCollection;
    //UI characters references
    List<UICharacter> AllCharacters;
    //Current player character
    UICharacter CurrentChar;
    //Default UI character reference
    public UICharacter DefaultUIChar;
    //Scroll bar reference (with the list of characters)
    public Scrollbar CharactersScroll;

    //UI Stats of the selected character
    public Image PreviewAvatar;
    public Image Emblem;
    public Text CharName;
    public Text CharTitle;
    public Text CharDesc;
    public Text CharAka;
    public Text CharLvl;
    public Text CosRunsAv;
    public Text[] SkillNames = new Text[4];
    public Text[] SkillDesc = new Text[4];
    [Header("TMP TEXTS")]
    public Text CharName2;
    public Text CharTitle2;
    public Text CharDesc2;
    public Text CharAka2;
    public Text CharLvl2;
    public Text CosRunsAv2;
    public Text[] SkillNames2 = new Text[4];
    public Text[] SkillDesc2 = new Text[4];

    

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the UI characters list
        AllCharacters = new List<UICharacter>();
        //Get the player collection data
        PlayerCollection = GlobalManager.GMD.GetUserCollection();
        //Get the current player character
        NFTsCharacter pCharacter = GlobalManager.GMD.GetUserCharacter();
        PlayerCollection.ChangeDeckFaction(pCharacter); //||||||||||||||||||||||||||||||||||||||||| Agregue esta linea para Local Selección de caracter

        //Show the UI characters from the player collection characters data
        foreach (NFTsCharacter character in PlayerCollection.Characters.OrderByDescending(o => o.LocalID))
        {
            if (!PlayerCollection.FactionDeckExist((Factions)character.Faction))
                continue;

            UICharacter uichar = Instantiate(DefaultUIChar.gameObject, DefaultUIChar.transform.parent).GetComponent<UICharacter>();
            uichar.SetData(character);
            uichar.gameObject.SetActive(true);
            uichar.transform.SetAsFirstSibling();
            AllCharacters.Add(uichar);
        }

        //Sets the player selected character, if is null, select the first one of the list
        CurrentChar = AllCharacters.FirstOrDefault(f => f.GetData() == pCharacter);
        if (CurrentChar == null)
        {
            CurrentChar = AllCharacters[0];
        }
        //Update the UI of the selected character
        UpdateUIInfo();
    }

    //Selects and changes the current player´s character
    public void BtnSelectCharacter(UICharacter character)
    {
        CurrentChar.SetSelection(false);
        NFTsCharacter nFTsCharacter = character.GetData();
        CurrentChar = character;
        UpdateUIInfo();
        GlobalManager.GMD.SetUserCharacter(nFTsCharacter.ID);
        
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.Character);
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.CharacterName);

        if (GlobalManager.GMD.IsProductionWeb())
        {
            GameNetwork.JSSavePlayerCharacter(CurrentChar.GetData().ID);
        }
        else
        {
            PlayerPrefs.SetInt("CharacterSaved", CurrentChar.GetData().ID);
        }
    }

    //Updates the UI info of the selected character
    public void UpdateUIInfo()
    {
        CurrentChar.SetSelection(true);
        string key = CurrentChar.GetData().KeyId;
        PreviewAvatar.sprite = CurrentChar.GetData().IconSprite;
        Emblem.sprite = ResourcesServices.LoadCharacterEmblem(key);
        CharName.text = Lang.GetEntityName(key);
        CharDesc.text = Lang.GetEntityDescription(key);
        CharTitle.text = Lang.GetText($"{key}_title");
        CharAka.text = Lang.GetText($"{key}_aka");
        CharLvl.text = $"{Lang.GetText("mn_level")} 1";
        CosRunsAv.text = $"{Lang.GetText("mn_cr_left")} 0";
        for(int i=0; i<4; i++)
        {
            SkillNames[i].text = Lang.GetText("mn_comingsoon");
            SkillDesc[i].text = Lang.GetText("mn_comingsoon");
        }
    }

    //Scrolls to the right limit of the characters list
    public void GoLimit()
    {
        CharactersScroll.value = 1;
    }
}
