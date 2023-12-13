using UnityEngine;

[System.Serializable]
public class Outcome
{
    public string message;
    public string buttonMessage;
    public string type;
    public string choiceID;

    // Methods
    public void InitializeFromJSON(OutcomeData outcomeData)
    {
        // Set properties based on the deserialized data
        message = outcomeData.Message;
        buttonMessage = outcomeData.ButtonMessage;
        type = outcomeData.Type;
        choiceID = outcomeData.ChoiceID;

        // Debugging line
        Debug.Log("Outcome Initialized: " + message);
    }
}
