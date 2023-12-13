using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public class EventHandling : MonoBehaviour
{
    [System.Serializable]
    public class EventData
    {
        public string EventID;
        public string EventType;
        public string Description;
        public string Tip;
        public string CostIncrementPercent;
        public string LossPercentage;
    }

    private List<EventData> events = new List<EventData>();
    private int currentEventIndex = 0;

    public TextMeshProUGUI Event_Name;
    public TextMeshProUGUI Event_Description;
    public TextMeshProUGUI Event_Tip;
    public TextMeshProUGUI Event_Cost;
    public TextMeshProUGUI Event_Loss;

    void Start()
    {
        LoadJson("Events");
        LoadEvent(currentEventIndex);
    }

    void LoadJson(string jsonFileName)
    {
        string filePath = Path.Combine("Assets/Resources/SO", jsonFileName + ".json");
        Debug.Log("Loading JSON file from path: " + filePath);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            events = JsonConvert.DeserializeObject<List<EventData>>(jsonContent);
        }
        else
        {
            Debug.LogError("Failed to load JSON content for file: " + jsonFileName);
        }
    }

    void LoadEvent(int index)
    {
        if (index >= 0 && index < events.Count)
        {
            EventData currentEvent = events[index];
            Event_Name.text = currentEvent.EventType;
            Event_Description.text = currentEvent.Description;
            Event_Tip.text = "Tip: " + currentEvent.Tip;
            Event_Cost.text = "Cost Increment: " + currentEvent.CostIncrementPercent;
            Event_Loss.text = "Loss Percentage: " + currentEvent.LossPercentage;
        }
        else
        {
            Debug.LogError("Invalid event index: " + index);
        }
    }
}
