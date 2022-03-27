using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICharacters : MonoBehaviour
{
    public UICharacter DefaultCharacter;

    public Scrollbar CharactersScroll;

    public RectTransform EmptyLeft;
    public RectTransform EmptyRigth;

    public Text CurrentDescription;

    Resolution res;

    public UserCollection PlayerCollection;
    List<UICharacter> AllCharacters;
    UICharacter CurrentChar;
    UICharacter FrontChar;

    float slicePorcent;

    // Start is called before the first frame update
    void Start()
    {
        AllCharacters = new List<UICharacter>();
        PlayerCollection = GameData.GetUserCollection();
        NFTsCharacter pCharacter = GameData.GetUserCharacter();

        res = Screen.currentResolution;
        ResizeEmptySides();

        int nChars = PlayerCollection.Characters.Count;
        slicePorcent = 1f / ((float)nChars - 1);

        for (int i=0; i< nChars; i++)
        {
            UICharacter uICharacter = Instantiate(DefaultCharacter, DefaultCharacter.transform.parent).GetComponent<UICharacter>();
            uICharacter.SetData(PlayerCollection.Characters[i]);
            uICharacter.AlphaFactor = slicePorcent * i;
            uICharacter.DeltaFactor = nChars / 2;
            uICharacter.RefScroll = CharactersScroll;
            AllCharacters.Add(uICharacter);
            if (pCharacter.KeyId == PlayerCollection.Characters[i].KeyId)
            {
                CurrentChar = uICharacter;
                uICharacter.SelectChar();
            }
        }

        DefaultCharacter.gameObject.SetActive(false);
        EmptyRigth.transform.SetAsLastSibling();
        
        CharactersScroll.value = 0f;
        FrontChar = AllCharacters[0];
        CurrentDescription.text = Lang.GetEntityDescription(FrontChar.GetData().KeyId);
    }

    // Update is called once per frame
    void Update()
    {
        if (res.width != Screen.currentResolution.width || res.height != Screen.currentResolution.height)
        {
            ResizeEmptySides();

            res = Screen.currentResolution;
        }
    }

    public void BtnSelectCharacter()
    {
        CurrentChar.DeselectChar();
        CurrentChar = FrontChar;
        NFTsCharacter nFTsCharacter = CurrentChar.GetData();
        GameData.SetUserCharacter(nFTsCharacter.KeyId);
        CurrentChar.SelectChar();
        if (!GameData.DebugMode)
        {
            GameNetwork.JSSavePlayerCharacter(JsonConvert.SerializeObject(CurrentChar.GetData()));
        }
    }

    public void MoveScroll()
    {
        UICharacter current = AllCharacters.OrderBy(o => o.CurrentDelta).FirstOrDefault();
        if (FrontChar != current)
        {
            FrontChar = current;
            CurrentDescription.text = Lang.GetEntityDescription(FrontChar.GetData().KeyId);
        }
    }

    public void MoveNextCharacter()
    {
        CharactersScroll.value += slicePorcent;
        if (CharactersScroll.value > 1f)
        {
            CharactersScroll.value = 1f;
        }
        foreach(UICharacter uICharacter in AllCharacters)
        {
            uICharacter.UpdateDelta();
        }
        MoveScroll();
    }

    public void MovePrevCharacter()
    {
        CharactersScroll.value -= slicePorcent;
        if (CharactersScroll.value < 0f)
        {
            CharactersScroll.value = 0f;
        }
        foreach (UICharacter uICharacter in AllCharacters)
        {
            uICharacter.UpdateDelta();
        }
        MoveScroll();
    }

    void ResizeEmptySides()
    {
        EmptyLeft.sizeDelta = new Vector2((Screen.width - 480) * 0.5f, EmptyLeft.sizeDelta.y);
        EmptyRigth.sizeDelta = new Vector2((Screen.width - 480) * 0.5f, EmptyLeft.sizeDelta.y);
    }
}
