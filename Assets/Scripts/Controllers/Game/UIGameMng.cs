using UnityEngine;
using UnityEngine.UI;

public class UIGameMng : MonoBehaviour
{
    //Screens objects references
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject ResultsScreen;

    //UI modules objects references
    public GameObject TopMidInfo;
    public GameObject DeckPanel;

    //Cards objects references
    public UIGameCard[] UIDeck = new UIGameCard[8];

    //Time, energy number and energy bar references
    public Text TimeOut;
    public Text EnergyLabel;
    public Image EnergyBar;

    //Trigger grid for deploy cards
    public GameObject AreaDeploy;

    //Results Metrics text references
    public Text MTxtEnergyUsed;
    public Text MTxtEnergyGenerated;
    public Text MTxtEnergyWasted;
    public Text MTxtEnergyChargeRatePerSec;
    public Text MTxtDamage;
    public Text MTxtKills;
    public Text MTxtDeploys;
    public Text MTxtSecRemaining;
    public Text MTxtScore;

    //Players panels references
    public UIPlayerGameInfo[] Players = new UIPlayerGameInfo[2];

    //HP and Shield Colors for every team
    Color FriendHpBarColor;
    Color FriendShieldBarColor;

    Color EnemyHpBarColor;
    Color EnemyShieldBarColor;

    private void Awake()
    {
        //Set the UI controller
        GameMng.UI = this;
        //Init the hp and shield colors
        FriendHpBarColor = new Color(0.25f, 1f, 0.28f, 1f);
        FriendShieldBarColor = new Color(0.25f, 0.66f, 1f, 1f);
        EnemyHpBarColor = new Color(1f, 0.25f, 0.25f, 1f);
        EnemyShieldBarColor = new Color(1f, 0.8f, 0.25f, 1f);
    }

    private void Start()
    {
        //Init the UI info of the player
        Players[GameMng.P.ID-1].InitInfo(GameMng.PlayerData, GameMng.PlayerProgress, GameMng.PlayerCharacter);
        //Inits the enemys info depending the match
        switch(GameData.CurrentMatch)
        {
            case Match.tutorial:
                {
                    Players[1].InitInfo(new UserGeneral()
                    {
                        NikeName = "Sotzeer",
                        WalletId = string.Empty,
                        Level = 99,
                        Xp = 0,
                        Avatar = 4,
                        CharacterKey = "Chr_4"
                    });
                }
                break;
            case Match.bots:
                {
                    Players[1].InitInfo(new UserGeneral()
                    {
                        NikeName = "CosmicBoot",
                        WalletId = string.Empty,
                        Level = 99,
                        Xp = 0,
                        Avatar = 2,
                        CharacterKey = "Chr_4"
                    });
                }
                break;
            case Match.multi:
                {
                    Players[GameMng.P.ID == 1 ? 1 : 0].InitInfo(GameData.GetVsUser());
                }
                break;
        }
    }

    //Shows the game over screen
    public void SetGameOver(Team winner)
    {
        TopMidInfo.SetActive(false);
        DeckPanel.SetActive(false);

        ResultsScreen.SetActive(true);
        if (winner == GameMng.P.MyTeam)
        {
            VictoryScreen.SetActive(true);
        } else
        {
            DefeatScreen.SetActive(true);
        }

        GameMng.MT.CalculateLastMetrics(winner);
        UpdateResults();
    }
    //Init the UI Cards
    public void InitGameCards(GameCard[] gameCards)
    {
        for (int i=0; i<gameCards.Length; i++)
        {
            UIDeck[i].SpIcon.sprite = gameCards[i].Icon;
            UIDeck[i].EnergyCost = gameCards[i].EnergyCost;
            UIDeck[i].TextCost.text = gameCards[i].EnergyCost.ToString();
        }
    }
    //Update the UI time
    public void UpdateTimeOut(string newtime)
    {
        TimeOut.text = newtime;
    }
    //Shows a card has selected
    public void SelectCard(int idc)
    {
        UIDeck[idc].SetSelection(true);
        AreaDeploy.SetActive(true);
    }
    //Shows all the cards has deselected
    public void DeselectCards()
    {
        foreach (UIGameCard card in UIDeck)
        {
            card.SetSelection(false);
        }
        AreaDeploy.SetActive(false);
    }
    //Update the energy bar and text
    public void UpdateEnergy(float energy, float max)
    {
        EnergyLabel.text = energy.ToString(energy == max ? "F0" : "F1");
        EnergyBar.fillAmount = energy / max;

        foreach (UIGameCard card in UIDeck)
        {
            card.TextCost.color = energy >= card.EnergyCost ? Color.white : Color.red;
        }
    }
    //Update the results text panel with the game metrics
    public void UpdateResults()
    {
        MTxtEnergyUsed.text = GameMng.MT.GetEnergyUsed().ToString();
        MTxtEnergyGenerated.text = GameMng.MT.GetEnergyGenerated().ToString("F0");
        MTxtEnergyWasted.text = GameMng.MT.GetEnergyWasted().ToString("F0");
        MTxtEnergyChargeRatePerSec.text = GameMng.MT.GetEnergyChargeRatePerSec().ToString()+"/s";

        MTxtDamage.text = GameMng.MT.GetDamage().ToString();
        MTxtKills.text = GameMng.MT.GetKills().ToString();
        MTxtDeploys.text = GameMng.MT.GetDeploys().ToString();
        MTxtSecRemaining.text = GameMng.MT.GetSecRemaining().ToString()+" s";

        MTxtScore.text = GameMng.MT.GetScore().ToString();
    }
    //Returns the HP color for a unit
    public Color GetHpBarColor(bool isEnnemy)
    {
        return isEnnemy ? EnemyHpBarColor : FriendHpBarColor;
    }
    //Returns the Shield color for a unit
    public Color GetShieldBarColor(bool isEnnemy)
    {
        return isEnnemy ? EnemyShieldBarColor : FriendShieldBarColor;
    }
}
