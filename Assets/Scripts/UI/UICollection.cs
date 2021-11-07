using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum CardOrder
{
    Name,
    Cost,
    Rarity
}

public enum CardClass
{
    Ship,
    Station,
    Skill
}

public class UICollection : MonoBehaviour
{
    public UICard[] Deck = new UICard[8];

    public UICard CardCollection;

    public UICardDetail CardPreview;

    [HideInInspector]
    public UICard CurrentSelected;

    UICard DragingCard;
    UICard EnterCard;

    List<UICard> AllCards;

    UserCollection PlayerCollection;

    public UICardDrag DragIcon;

    public Dropdown DD_OrderBy;

    List<CardClass> ClassFilter;

    public bool FilterShips { get; set; }
    public bool FilterStations { get; set; }
    public bool FilterSkills { get; set; }
    public string FilterSearch { get; set; }
    public int OrderBy { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        FilterShips = true;
        FilterStations = true;
        FilterSkills = true;

        OrderBy = 0;
        FilterSearch = string.Empty;
        ClassFilter = new List<CardClass>();
        ClassFilter.Add(CardClass.Ship);
        ClassFilter.Add(CardClass.Station);
        ClassFilter.Add(CardClass.Skill);

        DD_OrderBy.ClearOptions();
        DD_OrderBy.AddOptions(new List<Dropdown.OptionData>() {
            new Dropdown.OptionData { text = Lang.GetText("mn_name")},
            new Dropdown.OptionData { text = Lang.GetText("mn_cost")},
            new Dropdown.OptionData { text = Lang.GetText("mn_rarity")}
        });

        PlayerCollection = GameData.GetUserCollection();

        AllCards = new List<UICard>();

        List<NFTsCard> Sorted = PlayerCollection.Cards.OrderBy(f => f.Name).ToList();

        foreach (NFTsCard nFTsCard in Sorted)
        {
            if (PlayerCollection.Deck.Contains(nFTsCard))
            {
                continue;
            }
            UICard card = Instantiate(CardCollection.gameObject, CardCollection.transform.parent).GetComponent<UICard>();
            AllCards.Add(card);
            card.SetData(nFTsCard);
        }

        if (PlayerCollection.Deck.Count == 8)
        {
            for(int i=0; i<PlayerCollection.Deck.Count; i++)
            {
                Deck[i].SetData(PlayerCollection.Deck[i]);
            }
        }

        CardCollection.gameObject.SetActive(false);

        SelectCard(AllCards[0]);
    }

    public void SelectCard(UICard card)
    {
        if (card.DeckSlot == -1 && EnterCard != null)
        {
            return;
        }

        if (CurrentSelected != null)
        {
            CurrentSelected.DeselectCard();
        }
        CurrentSelected = card;
        card.SelectCard();
        CardPreview.SetData(card.GetData());
    }
    
    public void DragCard(UICard card)
    {
        DragingCard = card;
        DragIcon.gameObject.SetActive(true);
        DragIcon.Icon.sprite = card.Icon.sprite;
        DragIcon.transform.position = Input.mousePosition;
    }

    public void DropCard()
    {
        if (EnterCard!= null && DragingCard != null)
        {
            NFTsCard todeck = DragingCard.GetData();
            NFTsCard tocol = EnterCard.GetData();
            PlayerCollection.Deck[EnterCard.DeckSlot] = todeck;

            EnterCard.SetData(todeck);
            DragingCard.SetData(tocol);

            SortAndFilterCollection();
        }
        
        DragingCard = null;
        DragIcon.gameObject.SetActive(false);
    }

    public void DeckEnterDrop(UICard card)
    {    
        EnterCard = card;
    }

    public void ClearEnterDrop(UICard card)
    {
        EnterCard = null;
    }

    public void SortAndFilterCollection()
    {
        ClassFilter.Clear();
        if (FilterShips)
            ClassFilter.Add(CardClass.Ship);
        if (FilterStations)
            ClassFilter.Add(CardClass.Station);
        if (FilterSkills)
            ClassFilter.Add(CardClass.Skill);

        List<NFTsCard> Sorted;

        switch((CardOrder)OrderBy)
        {
            case CardOrder.Cost:
                {
                    Sorted = PlayerCollection.Cards.OrderByDescending(f => f.EnergyCost).ToList();
                }
                break;
            case CardOrder.Rarity:
                {
                    Sorted = PlayerCollection.Cards.OrderByDescending(f => f.Rarity).ToList();
                }
                break;
            default:
                {
                    Sorted = PlayerCollection.Cards.OrderByDescending(f => f.Name).ToList();
                }
                break;
        }

        for(int i = 0; i < Sorted.Count; i++)
        {
            UICard card = AllCards.FirstOrDefault(f => f.KeyName == Sorted[i].KeyId);
            if (card)
            {
                card.transform.SetSiblingIndex(0);
            }
        }

        foreach (UICard uICard in AllCards)
        {
            uICard.gameObject.SetActive(
                ClassFilter.Contains(uICard.TypeCard) 
                && (string.IsNullOrWhiteSpace(FilterSearch) || uICard.NameCard.ToLower().Contains(FilterSearch.ToLower()))
                );
        }
    }
}
