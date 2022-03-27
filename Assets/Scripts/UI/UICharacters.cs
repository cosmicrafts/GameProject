using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICharacters : MonoBehaviour
{
    public UserCollection PlayerCollection;
    List<UICharacter> AllCharacters;
    UICharacter CurrentChar;

    public UICharacter DefaultUIChar;

    public Scrollbar CharactersScroll;

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

    // Start is called before the first frame update
    void Start()
    {
        AllCharacters = new List<UICharacter>();
        PlayerCollection = GameData.GetUserCollection();
        NFTsCharacter pCharacter = GameData.GetUserCharacter();

        foreach (NFTsCharacter character in PlayerCollection.Characters.OrderByDescending(o => o.CharacterId))
        {
            UICharacter uichar = Instantiate(DefaultUIChar.gameObject, DefaultUIChar.transform.parent).GetComponent<UICharacter>();
            uichar.SetData(character);
            uichar.gameObject.SetActive(true);
            uichar.transform.SetAsFirstSibling();
            AllCharacters.Add(uichar);
        }

        CurrentChar = AllCharacters.FirstOrDefault(f => f.GetData() == pCharacter);
        if (CurrentChar == null)
        {
            CurrentChar = AllCharacters[0];
        }
        UpdateUIInfo();
    }

    public void BtnSelectCharacter(UICharacter character)
    {
        CurrentChar.SetSelection(false);
        NFTsCharacter nFTsCharacter = character.GetData();
        CurrentChar = character;
        UpdateUIInfo();
        GameData.SetUserCharacter(nFTsCharacter.KeyId);

        if (!GameData.DebugMode)
        {
            GameNetwork.JSSavePlayerCharacter(JsonConvert.SerializeObject(CurrentChar.GetData()));
        }
    }

    public void UpdateUIInfo()
    {
        CurrentChar.SetSelection(true);
        string key = CurrentChar.GetData().KeyId;
        PreviewAvatar.sprite = ResourcesServices.LoadCharacterIcon(CurrentChar.GetData().Icon);
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

    public void GoLimit()
    {
        CharactersScroll.value = 1;
    }
}
