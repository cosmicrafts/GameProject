using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDisplay : MonoBehaviour
{
    public Conversation conversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;

    private int activeLineIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            AdvanceConversation();
        }
    }

    void AdvanceConversation()
    {
        if(activeLineIndex < conversation.lines.Length)
        {
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            activeLineIndex = 0;
        }
    }

    // assumption: only two characters
    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if(speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
            /* speakerUILeft.Show();
            speakerUIRight.Hide();

            speakerUILeft.Dialog = string.Empty;
            foreach(char ch in line.text)
            {
                speakerUILeft.Dialog = speakerUILeft.Dialog + ch ;
            } */
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
    }

    void SetDialog(SpeakerUI activeSpeakerUI,
                    SpeakerUI inactiveSpeakerUI,
                    string text)
    {
        activeSpeakerUI.Dialog = text;
        //activeSpeakerUI.Dialog = StartCoroutine(LetraPorLetra(text));
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();
    }

    // recorer al array para mostrar letra por letra
    private IEnumerator LetraPorLetra(string text)
    {
        string dialogo = string.Empty;
        foreach(char ch in text)
        {
            dialogo += ch;
            yield return new WaitForSeconds(0.05f);
           
        }
    }
}
