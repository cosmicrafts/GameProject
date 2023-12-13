using System.Collections.Generic;

[System.Serializable]
public class EventComponentData
{
    public string EventID;
    public string EventType;
    public string Description;
    public string Tip;
    public string CostIncrementPercent;
    public string LossPercentage;
    public List<ChoiceData> Choices;
}
