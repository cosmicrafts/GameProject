using System;
using System.Collections.Generic;

[System.Serializable]
public class ChoiceData
{
    public string ChoiceID;
    public string Description;
    public string AwaitingMessage;
    public Requirements Requirements; // Make sure you have the necessary using directive
    public string RiskLevel;
    public List<string> OutcomeID; // Make sure you have the necessary using directive
    public string EventID;
}
