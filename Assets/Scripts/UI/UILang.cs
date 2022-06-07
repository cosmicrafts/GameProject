using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UILang : MonoBehaviour
{
    //The key of the string
    public string ID = "None";
    //Defines if the text needs to update when gets enable
    bool okstart = false;

    //Loads and shows the string when begings
    void Start()
    {
        SetMyText();
        okstart = true;
    }

    //Loads and shows the string
    public void SetMyText()
    {
        GetComponent<Text>().text = Lang.GetText(ID);
    }

    //Loads and shows the string if is required
    void OnEnable()
    {
        if (okstart)
        {
            SetMyText();
            okstart = false;
        }
    }
}
