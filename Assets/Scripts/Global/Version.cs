using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Version : MonoBehaviour
{
    //Show the current game version (requires a TEXT component)
    void Start()
    {
        GetComponent<Text>().text = GameData.GetVersion();
    }
}
