using System;
using System.Runtime.InteropServices;
using Candid;
using CanisterPK.CanisterStats.Models;
using EdjCase.ICP.Candid.Models;
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
   
   
    public async void UpdateResults(bool isWin)
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
        
        BasicStats basicStats = new BasicStats();
        basicStats.EnergyUsed = GameManager.MT.GetEnergyUsed();
        basicStats.EnergyGenerated = GameManager.MT.GetEnergyGenerated();
        basicStats.EnergyWasted = GameManager.MT.GetEnergyWasted();
        basicStats.EnergyChargeRate = GameManager.MT.GetEnergyChargeRatePerSec();
        basicStats.XpEarned = GameManager.MT.GetScore();
        basicStats.DamageDealt = GameManager.MT.GetDamage();
        basicStats.DamageTaken = GameManager.MT.GetDamageReceived(); 
        basicStats.DamageCritic = GameManager.MT.GetDamageCritic();
        basicStats.DamageEvaded = GameManager.MT.GetDamageEvaded();
        basicStats.Kills = GameManager.MT.GetKills();
        basicStats.Deploys = GameManager.MT.GetDeploys();
        basicStats.SecRemaining = GameManager.MT.GetSecRemaining();
        basicStats.WonGame = isWin;
        basicStats.Faction = (UnboundedUInt)GlobalGameData.Instance.GetUserCharacter().Faction;
        basicStats.CharacterID = GlobalGameData.Instance.GetUserCharacter().KeyId;
        basicStats.GameMode = (UnboundedUInt)(int)GlobalGameData.Instance.GetConfig().currentMatch;
        basicStats.BotMode = (UnboundedUInt) PlayerPrefs.GetInt("BotMode");
        basicStats.BotDifficulty = (UnboundedUInt) PlayerPrefs.GetInt("Dificulty");
        
        var statsSend = await CandidApiManager.Instance.CanisterStats.SaveFinishedGame((UnboundedUInt) PunNetworkManager.NetworkManager.gameId, basicStats);
        Debug.Log(statsSend);   
    }
   
   
}
