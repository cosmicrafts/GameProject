using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Random = UnityEngine.Random; // To avoid confusion with System.Random

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
    public TextMeshProUGUI Event_Name;
    public TextMeshProUGUI Event_Description;
    public Button Choice1_Btn;
    public Button Choice2_Btn;
    public TextMeshProUGUI Outcome;

    private List<CosmicEvent> events = new List<CosmicEvent>();
    private int currentEventIndex = 0;
    private TokenUIHandler tokenUIHandler; // Reference to TokenUIHandler

    void Start()
    {
        LoadJson();
        SetupButtonListeners();
        tokenUIHandler = FindObjectOfType<TokenUIHandler>();
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

            Choice1_Btn.gameObject.SetActive(currentEvent.choices.Count > 0);
            Choice2_Btn.gameObject.SetActive(currentEvent.choices.Count > 1);

            if (currentEvent.choices.Count > 0)
                Choice1_Btn.GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.choices[0].choiceDescription;
            if (currentEvent.choices.Count > 1)
                Choice2_Btn.GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.choices[1].choiceDescription;

            Debug.Log("Loaded Event: " + currentEvent.title);
        }
        else
        {
            Debug.LogError("Invalid event index: " + index);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        DeductEventCost();
        ApplyChoiceEffects(events[currentEventIndex].choices[choiceIndex]);
        LoadNextEvent();
    }

    void ApplyChoiceEffects(Choice choice)
    {
        // Randomly gain or lose tokens
        foreach (var outcomeIndex in choice.outcomeIndexes)
        {
            RandomlyAdjustTokenBalance("Energy");
            RandomlyAdjustTokenBalance("Matter");
        }
    }

    void RandomlyAdjustTokenBalance(string tokenName)
    {
        bool gainTokens = Random.Range(0, 2) == 0; // 50% chance
        int tokenAmount = Random.Range(1, 9); // Random amount between 1 and 8

        int currentBalance = int.Parse(tokenUIHandler.GetTokenBalance(tokenName));
        int updatedBalance = gainTokens ? currentBalance + tokenAmount : currentBalance - tokenAmount;

        tokenUIHandler.UpdateTokenData(tokenName, updatedBalance.ToString());
    }

    void DeductEventCost()
    {
        // Deduct 3 Energy and 2 Matter as cost of starting an event
        tokenUIHandler.UpdateTokenData("Energy", (int.Parse(tokenUIHandler.GetTokenBalance("Energy")) - 3).ToString());
        tokenUIHandler.UpdateTokenData("Matter", (int.Parse(tokenUIHandler.GetTokenBalance("Matter")) - 2).ToString());
    }

    void LoadNextEvent()
    {
        int nextEventIndex;
        do
        {
            nextEventIndex = Random.Range(0, events.Count);
        }
        while (nextEventIndex == currentEventIndex);

        currentEventIndex = nextEventIndex;
        LoadEvent(currentEventIndex);
    }

    void SetupButtonListeners()
    {
        Choice1_Btn.onClick.AddListener(delegate { MakeChoice(0); });
        Choice2_Btn.onClick.AddListener(delegate { MakeChoice(1); });
    }
}
