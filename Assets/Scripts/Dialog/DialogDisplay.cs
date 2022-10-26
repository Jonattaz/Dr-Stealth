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
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            AdvanceConversation();
        
        }
           
    }


  public void AdvanceConversation() 
    {
        if (activeLineIndex < conversation.lines.Length) 
        {
            // Bool that inform if the player is talking = true
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            activeLineIndex = 0;
             // Bool that inform if the player is talking = false
        }
    }

    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character)) 
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);

        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
    }

    void SetDialog
    (
        SpeakerUI activeSpeakerUI,
        SpeakerUI inactiveSpeakerUI,
        string text) 
    {
        activeSpeakerUI.Dialog = text;
        activeSpeakerUI.Show();

        inactiveSpeakerUI.Dialog = text;
        inactiveSpeakerUI.Hide();
    
    }
}
