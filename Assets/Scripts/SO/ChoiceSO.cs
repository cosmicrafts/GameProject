using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChoiceSO", menuName = "ScriptableObjects/ChoiceSO", order = 2)]
public class ChoiceSO : ScriptableObject
{
    public string choiceID;
    public string description;
    public string awaitingMessage;
    public Dictionary<string, int> tokenRequirements; // For simplicity, tokens are represented as string-int pairs
    public List<string> nftRequirements;
    public List<OutcomeSO> outcomes;
}
