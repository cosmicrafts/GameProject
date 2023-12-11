using UnityEngine;

[CreateAssetMenu(fileName = "OutcomeSO", menuName = "ScriptableObjects/OutcomeSO", order = 3)]
public class OutcomeSO : ScriptableObject
{
    public string message;
    public string buttonMessage;
    public OutcomeType outcomeType;
}

public enum OutcomeType
{
    Positive,
    Neutral,
    Negative
}
