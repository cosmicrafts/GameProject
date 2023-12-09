using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EventList
{
    public List<CosmicEvent> events;
}

[System.Serializable]
public class CosmicEvent
{
    public string title;
    public string description;
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public string choiceDescription;
    public List<int> outcomeIndexes; // List of potential outcome indexes
}

public class EventHandling : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI Event_Name;
    public TextMeshProUGUI Event_Description;
    public Button Choice1_Btn;
    public Button Choice2_Btn;
    public TextMeshProUGUI Outcome;

    private List<CosmicEvent> events = new List<CosmicEvent>();
    private int currentEventIndex = 0;

    void Start()
    {
        LoadJson();
        SetupButtonListeners();
    }

    void LoadJson()
    {
        TextAsset fileData = Resources.Load<TextAsset>("4X/Events");
        if (fileData != null)
        {
            EventList eventList = JsonUtility.FromJson<EventList>(fileData.text);
            events = eventList.events;
            if (events.Count > 0)
            {
                LoadEvent(currentEventIndex);
            }
            else
            {
                Debug.LogError("Events list is empty!");
            }
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }

    void LoadEvent(int index)
    {
        if (index >= 0 && index < events.Count)
        {
            CosmicEvent currentEvent = events[index];
            Event_Name.text = currentEvent.title;
            Event_Description.text = currentEvent.description;

            if (currentEvent.choices.Count > 0)
            {
                Choice1_Btn.gameObject.SetActive(true);
                Choice1_Btn.GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.choices[0].choiceDescription;
            }
            else
            {
                Choice1_Btn.gameObject.SetActive(false);
            }

            if (currentEvent.choices.Count > 1)
            {
                Choice2_Btn.gameObject.SetActive(true);
                Choice2_Btn.GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.choices[1].choiceDescription;
            }
            else
            {
                Choice2_Btn.gameObject.SetActive(false);
            }

            Debug.Log("Loaded Event: " + currentEvent.title);
        }
        else
        {
            Debug.LogError("Invalid event index: " + index);
        }
    }

        public PlayerInventory playerInventory;
        public void MakeChoice(int choiceIndex)
    {
        int nextEventIndex;
        do
        {
            nextEventIndex = Random.Range(0, events.Count); // Randomly select an event index
        } 
        while (nextEventIndex == currentEventIndex); // Ensure the next event is different from the current one

        
        
        currentEventIndex = nextEventIndex;
        LoadEvent(currentEventIndex);
    }

    void SetupButtonListeners()
    {
        Choice1_Btn.onClick.AddListener(delegate { MakeChoice(0); });
        Choice2_Btn.onClick.AddListener(delegate { MakeChoice(1); });
    }
}
