using UnityEngine;
using System.Collections.Generic;

public class EventComponent : MonoBehaviour
{
    // Properties
    public string eventID;
    public string eventType;
    public string description;
    public string tip;
    public string costIncrementPercent;
    public string lossPercentage;

    // Choices for the event
    public List<Choice> choices;

    // Methods
    public void InitializeFromJSON(string jsonPath)
    {
        // Load JSON data from the file
        string jsonData = System.IO.File.ReadAllText(jsonPath);

        // Deserialize JSON data into the EventComponentData
        EventComponentData eventData = JsonUtility.FromJson<EventComponentData>(jsonData);

        // Set properties based on the deserialized data
        eventID = eventData.EventID;
        eventType = eventData.EventType;
        description = eventData.Description;
        tip = eventData.Tip;
        costIncrementPercent = eventData.CostIncrementPercent;
        lossPercentage = eventData.LossPercentage;

        // Initialize choices
        choices = new List<Choice>();
        foreach (var choiceData in eventData.Choices)
        {
            Choice choice = new Choice();
            choice.InitializeFromJSON(choiceData);
            choices.Add(choice);
        }

        // Debugging line
        Debug.Log("EventComponent Initialized: " + eventType);
    }
}
