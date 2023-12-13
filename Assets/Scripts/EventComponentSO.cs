using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEventComponent", menuName = "ScriptableObjects/EventComponentSO", order = 1)]
public class EventComponentSO : ScriptableObject
{
    public string eventID;
    public string eventType;
    public string description;
    public string tip;
    public string costIncrementPercent;
    public string lossPercentage;

    // Other properties as needed...

    [HideInInspector]
    public ChoiceData[] choices; // Assuming you want to assign choices in the Unity Editor
}
