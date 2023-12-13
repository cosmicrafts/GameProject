using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice
{
    public string choiceID;
    public string description;
    public string awaitingMessage;
    public Requirements requirements;
    public string riskLevel;
    public List<string> outcomeIDs;
    public string eventID;

    public void InitializeFromJSON(ChoiceData choiceData)
{
    // Set properties based on the deserialized data
    choiceID = choiceData.ChoiceID;
    description = choiceData.Description;
    awaitingMessage = choiceData.AwaitingMessage;
    // Other property assignments...

    // Debugging line
    Debug.Log("Choice Initialized: " + description);
}

}