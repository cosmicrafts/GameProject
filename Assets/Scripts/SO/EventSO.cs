using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EventSO", menuName = "ScriptableObjects/EventSO", order = 1)]
public class EventSO : ScriptableObject
{
    public string eventID;
    public string eventType;
    public string description;
    public string tip;
    public List<ChoiceSO> choices;
}