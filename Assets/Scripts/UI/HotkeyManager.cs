using UnityEngine;
using UnityEngine.EventSystems; // Required for UI event handling

public class HotkeyManager : MonoBehaviour
{
    // References to the UIGameCard components
    private UIGameCard card0;
    private UIGameCard card1;
    private UIGameCard card2;
    private UIGameCard card3;
    private UIGameCard card4;
    private UIGameCard card5;
    private UIGameCard card6;
    private UIGameCard card7;

    // Use this for initialization
    void Start()
    {
        // Find the UIGameCard components by finding their parent GameObjects by name
        card0 = GameObject.Find("Card_0").GetComponent<UIGameCard>();
        card1 = GameObject.Find("Card_1").GetComponent<UIGameCard>();
        card2 = GameObject.Find("Card_2").GetComponent<UIGameCard>();
        card3 = GameObject.Find("Card_3").GetComponent<UIGameCard>();
        card4 = GameObject.Find("Card_4").GetComponent<UIGameCard>();
        card5 = GameObject.Find("Card_5").GetComponent<UIGameCard>();
        card6 = GameObject.Find("Card_6").GetComponent<UIGameCard>();
        card7 = GameObject.Find("Card_7").GetComponent<UIGameCard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            card0.OnDown?.Invoke(card0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            card1.OnDown?.Invoke(card1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            card2.OnDown?.Invoke(card2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            card3.OnDown?.Invoke(card3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            card4.OnDown?.Invoke(card4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            card5.OnDown?.Invoke(card5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            card6.OnDown?.Invoke(card6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            card7.OnDown?.Invoke(card7);
        }
    }
}
