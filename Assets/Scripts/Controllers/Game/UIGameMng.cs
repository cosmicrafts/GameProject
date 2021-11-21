using UnityEngine;
using UnityEngine.UI;

public class UIGameMng : MonoBehaviour
{
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject ResultsScreen;

    public GameObject TopMidInfo;
    public GameObject DeckPanel;

    public UIGameCard[] UIDeck = new UIGameCard[8];

    public Text TimeOut;
    public Text EnergyLabel;
    public Image EnergyBar;

    public Image PlayerCharacter;

    public GameObject AreaDeploy;

    public Text MTxtEnergyUsed;
    public Text MTxtEnergyGenerated;
    public Text MTxtEnergyWasted;
    public Text MTxtEnergyChargeRatePerSec;

    public Text MTxtDamage;
    public Text MTxtKills;
    public Text MTxtDeploys;
    public Text MTxtSecRemaining;

    public Text MTxtScore;

    private void Awake()
    {
        GameMng.UI = this;
    }

    private void Start()
    {
        
    }

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

    public void InitGameCards(GameCard[] gameCards)
    {
        for (int i=0; i<gameCards.Length; i++)
        {
            UIDeck[i].SpIcon.sprite = gameCards[i].Icon;
            UIDeck[i].EnergyCost = gameCards[i].EnergyCost;
            UIDeck[i].TextCost.text = gameCards[i].EnergyCost.ToString();
        }
    }

    public void UpdateTimeOut(string newtime)
    {
        TimeOut.text = newtime;
    }

    public void SelectCard(int idc)
    {
        UIDeck[idc].SpIcon.color = Color.green;
        AreaDeploy.SetActive(true);
    }

    public void DeselectCards()
    {
        foreach (UIGameCard card in UIDeck)
        {
            card.SpIcon.color = Color.white;
        }
        AreaDeploy.SetActive(false);
    }

    public void UpdateEnergy(float energy, float max)
    {
        EnergyLabel.text = energy.ToString(energy == max ? "F0" : "F1");
        EnergyBar.fillAmount = energy / max;

        foreach (UIGameCard card in UIDeck)
        {
            card.TextCost.color = energy >= card.EnergyCost ? Color.white : Color.red;
        }
    }

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

    public void SetPlayerCharacter(Sprite icon)
    {
        PlayerCharacter.gameObject.SetActive(true);
        PlayerCharacter.sprite = icon;
    }
}
