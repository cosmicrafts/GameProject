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
    
  
    //Shows the game over screen
    public void SetGameOver(bool isWin)
    {
        TopMidInfo.SetActive(false);
        DeckPanel.SetActive(false);

        ResultsScreen.SetActive(true);
        if (isWin) { VictoryScreen.SetActive(true); }
        else { DefeatScreen.SetActive(true); }
        
        UpdateResults(isWin);
    }
   
   
    public void UpdateResults(bool isWin)
    {
        MTxtEnergyUsed.text = GameManager.MT.GetEnergyUsed().ToString();
        MTxtEnergyGenerated.text = GameManager.MT.GetEnergyGenerated().ToString("F0");
        MTxtEnergyWasted.text = GameManager.MT.GetEnergyWasted().ToString("F0");
        MTxtEnergyChargeRatePerSec.text = GameManager.MT.GetEnergyChargeRatePerSec().ToString()+"/s";

        MTxtXpEarned.text = "+" + GameManager.MT.GetScore().ToString();//GameMng.MT.GetDamage().ToString();
            
        MTxtDamage.text = GameManager.MT.GetDamage().ToString();
        MTxtDamageReceived.text = GameManager.MT.GetDamageReceived().ToString();
        MTxtDamageCritic.text = GameManager.MT.GetDamageCritic().ToString();
        MTxtDamageEvaded.text = GameManager.MT.GetDamageEvaded().ToString();
        
        MTxtKills.text = GameManager.MT.GetKills().ToString();
        MTxtDeploys.text = GameManager.MT.GetDeploys().ToString();
        MTxtSecRemaining.text = GameManager.MT.GetSecRemaining().ToString()+" s";

        MTxtScore.text = GameManager.MT.GetScore().ToString();

        StaticsResults staticsResults = new StaticsResults();
        staticsResults.energyUsed = GameManager.MT.GetEnergyUsed();
        staticsResults.energyGenerated = GameManager.MT.GetEnergyGenerated();
        staticsResults.energyWasted = GameManager.MT.GetEnergyWasted();
        staticsResults.energyChargeRate = GameManager.MT.GetEnergyChargeRatePerSec();
        staticsResults.xpEarned = GameManager.MT.GetScore();
        staticsResults.damage = GameManager.MT.GetDamage();
        staticsResults.damageReceived = GameManager.MT.GetDamageReceived();
        staticsResults.damageCritic = GameManager.MT.GetDamageCritic();
        staticsResults.damageEvaded = GameManager.MT.GetDamageEvaded();
        staticsResults.kills = GameManager.MT.GetKills();
        staticsResults.deploys = GameManager.MT.GetDeploys();
        staticsResults.secRemaining = GameManager.MT.GetSecRemaining();
        staticsResults.isWin = isWin ? 1 : 0;
        staticsResults.faction = GlobalGameData.Instance.GetUserCharacter().Faction;
        staticsResults.characterID = GlobalGameData.Instance.GetUserCharacter().KeyId;
        staticsResults.gameMode = (int)GlobalGameData.Instance.GetConfig().currentMatch;
        staticsResults.botMode = PlayerPrefs.GetInt("BotMode");
        staticsResults.botDificult = PlayerPrefs.GetInt("Dificulty");
        
        string json = JsonUtility.ToJson(staticsResults);
        Debug.Log(json);
        
        
        //Mnadar info al canister  JS_SendStats(json);


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
        public int isWin;
        public int faction;
        public string characterID;
        public int gameMode;
        public int botMode;
        public int botDificult;
    }
    
   
}
