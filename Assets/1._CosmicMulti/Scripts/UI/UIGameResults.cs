using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
 * This is the in-game UI controller
 * Contains the UI references and functions for update them
 * Only controls the game's data, the UI for the players and the tutorial are in other scripts
 */
public class UIGameResults : MonoBehaviour
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

    

    //Results Metrics text references
    public TMP_Text MTxtEnergyUsed;
    public TMP_Text MTxtEnergyGenerated;
    public TMP_Text MTxtEnergyWasted;
    public TMP_Text MTxtEnergyChargeRatePerSec;
    public TMP_Text MTxtXpEarned;
    public TMP_Text MTxtDamage;
    public TMP_Text MTxtDeploys;
    public TMP_Text MTxtKills;
    public TMP_Text MTxtSecRemaining;
    public TMP_Text MTxtScore;
    
    public TMP_Text MTxtDamageReceived;
    public TMP_Text MTxtDamageCritic;
    public TMP_Text MTxtDamageEvaded;
    
    //Players panels references
    public UIGamePlayer[] Players = new UIGamePlayer[2];

 

    
  

    private void Start()
    {
        //Debug.Log("--UI STARTS--");
        //Init the UI info of the player
        Players[GameMng.P.ID-1].InitInfo(GameMng.PlayerData, GameMng.PlayerProgress, GameMng.PlayerCharacter);
        //Inits the enemys info depending the match
        switch(GlobalManager.GMD.CurrentMatch)
        {
           
            case Match.multi:
                {
                    Players[GameMng.P.ID == 1 ? 1 : 0].InitInfo(GlobalManager.GMD.GetVsUser());
                }
                break;
        }
        //Debug.Log("--UI END STARTS--");
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
   
   
    public void UpdateResults()
    {
        MTxtEnergyUsed.text = GameMng.MT.GetEnergyUsed().ToString();
        MTxtEnergyGenerated.text = GameMng.MT.GetEnergyGenerated().ToString("F0");
        MTxtEnergyWasted.text = GameMng.MT.GetEnergyWasted().ToString("F0");
        MTxtEnergyChargeRatePerSec.text = GameMng.MT.GetEnergyChargeRatePerSec().ToString()+"/s";

        MTxtXpEarned.text = "+" + GameMng.MT.GetScore().ToString();//GameMng.MT.GetDamage().ToString();
            
        MTxtDamage.text = GameMng.MT.GetDamage().ToString();
        MTxtDamageReceived.text = GameMng.MT.GetDamageReceived().ToString();
        MTxtDamageCritic.text = GameMng.MT.GetDamageCritic().ToString();
        MTxtDamageEvaded.text = GameMng.MT.GetDamageEvaded().ToString();
        
        MTxtKills.text = GameMng.MT.GetKills().ToString();
        MTxtDeploys.text = GameMng.MT.GetDeploys().ToString();
        MTxtSecRemaining.text = GameMng.MT.GetSecRemaining().ToString()+" s";

        MTxtScore.text = GameMng.MT.GetScore().ToString();

        StaticsResults staticsResults = new StaticsResults();
        staticsResults.energyUsed = GameMng.MT.GetEnergyUsed();
        staticsResults.energyGenerated = GameMng.MT.GetEnergyGenerated();
        staticsResults.energyWasted = GameMng.MT.GetEnergyWasted();
        staticsResults.energyChargeRate = GameMng.MT.GetEnergyChargeRatePerSec();
        staticsResults.xpEarned = GameMng.MT.GetScore();
        staticsResults.damage = GameMng.MT.GetDamage();
        staticsResults.damageReceived = GameMng.MT.GetDamageReceived();
        staticsResults.damageCritic = GameMng.MT.GetDamageCritic();
        staticsResults.damageEvaded = GameMng.MT.GetDamageEvaded();
        staticsResults.kills = GameMng.MT.GetKills();
        staticsResults.deploys = GameMng.MT.GetDeploys();
        staticsResults.secRemaining = GameMng.MT.GetSecRemaining();
        staticsResults.teamWinner = (int)GameMng.GM.Winner;
        staticsResults.faction = GameMng.PlayerCharacter.Faction;
        staticsResults.characterID = GameMng.PlayerCharacter.KeyId;
        staticsResults.gameMode = (int)GlobalManager.GMD.CurrentMatch;
        staticsResults.botMode = PlayerPrefs.GetInt("BotMode");
        staticsResults.botDificult = PlayerPrefs.GetInt("Dificulty");
        
        string json = JsonUtility.ToJson(staticsResults);
        Debug.Log(json);
        JS_SendStats(json);


    }
    [System.Serializable]
    public class StaticsResults
    {
        public float energyUsed;
        public float energyGenerated;
        public float energyWasted;
        public float energyChargeRate;
        public float xpEarned;
        public float damage;
        public float damageReceived;
        public float damageCritic;
        public float damageEvaded;
        public float kills;
        public float deploys;
        public float secRemaining;
        public int teamWinner;
        public int faction;
        public string characterID;
        public int gameMode;
        public int botMode;
        public int botDificult;
    }
    
   
}
