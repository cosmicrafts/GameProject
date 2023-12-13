using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using Random = UnityEngine.Random;

[System.Serializable]
public class EventList : ScriptableObject
{
    public List<CosmicEvent> events;
}

[System.Serializable]
public class CosmicEvent
{
    public EventSO eventSO;
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
    private TokenUIHandler tokenUIHandler;
    bool jsonLoaded = false;

    void Start()
    {
        Transform canvasTransform = transform.Find("Canvas");

        Event_Name = GetChildComponent<TextMeshProUGUI>(canvasTransform, "Event_Name");
        Event_Description = GetChildComponent<TextMeshProUGUI>(canvasTransform, "Event_Description");
        Choice1_Btn = GetChildComponent<Button>(canvasTransform, "Choice1_Btn");
        Choice2_Btn = GetChildComponent<Button>(canvasTransform, "Choice2_Btn");
        Outcome = GetChildComponent<TextMeshProUGUI>(canvasTransform, "Outcome");

        if (Event_Name == null || Event_Description == null || Choice1_Btn == null || Choice2_Btn == null || Outcome == null)
        {
            Debug.LogError("One or more UI components are not assigned.");
            return;
        }

        if (!jsonLoaded)
        {
            LoadJson("Events.json");
            jsonLoaded = true;
        }

        SetupButtonListeners();
        tokenUIHandler = FindObjectOfType<TokenUIHandler>();
    }

    void LoadJson(string jsonFileName)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonFileName);
        EventList eventList = Resources.Load<EventList>(fileNameWithoutExtension);

        if (eventList != null)
        {
            events = eventList.events;
        }
        else
        {
            Debug.LogError("Failed to load JSON content for file: " + jsonFileName);
        }
    }

    T GetChildComponent<T>(Transform parent, string componentName) where T : Component
    {
        Transform child = parent.Find(componentName);
        if (child != null)
        {
            T component = child.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError(componentName + " component is missing.");
            }
            return component;
        }
        else
        {
            Debug.LogError(componentName + " not found.");
            return null;
        }
    }

    void LoadEvent(int index)
    {
        if (index >= 0 && index < events.Count)
        {
            CosmicEvent currentEvent = events[index];
            Event_Name.text = currentEvent.eventSO.eventType;
            Event_Description.text = currentEvent.eventSO.description;

            Choice1_Btn.gameObject.SetActive(currentEvent.eventSO.choices.Count > 0);
            Choice2_Btn.gameObject.SetActive(currentEvent.eventSO.choices.Count > 1);

            if (currentEvent.eventSO.choices.Count > 0)
                Choice1_Btn.GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.eventSO.choices[0].description;
            if (currentEvent.eventSO.choices.Count > 1)
                Choice2_Btn.GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.eventSO.choices[1].description;
        }
        else
        {
            Debug.LogError("Invalid event index: " + index);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        DeductEventCost();
        ApplyChoiceEffects(events[currentEventIndex].eventSO.choices[choiceIndex]);
        LoadNextEvent();
    }

    void ApplyChoiceEffects(ChoiceSO choice)
    {
        foreach (var outcome in choice.outcomes)
        {
            RandomlyAdjustTokenBalance("Energy");
            RandomlyAdjustTokenBalance("Matter");
        }
    }

    void RandomlyAdjustTokenBalance(string tokenName)
    {
        bool gainTokens = Random.Range(0, 2) == 0;
        int tokenAmount = Random.Range(1, 9);

        int currentBalance = int.Parse(tokenUIHandler.GetTokenBalance(tokenName));
        int updatedBalance = gainTokens ? currentBalance + tokenAmount : currentBalance - tokenAmount;

        tokenUIHandler.UpdateTokenData(tokenName, updatedBalance.ToString());
    }

    void DeductEventCost()
    {
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
