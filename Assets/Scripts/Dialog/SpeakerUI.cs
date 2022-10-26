using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerUI : MonoBehaviour
{   
    // Nome completo do personagem
    public Text fullName;
  
    // Texto que irá receber a fala do personagem
    public Text dialog;

    // Váriavel que representa a classe Character
    private Character speaker;

    public Character Speaker 
    {
        get { return speaker; }
        set 
        {
            speaker = value;
            fullName.text = speaker.fullName;
        
        }
    }

    public string Dialog 
    {
        set { dialog.text = value; }
    
    }

    public bool HasSpeaker() 
    {
        return speaker != null;
    
    }

    public bool SpeakerIs(Character character) 
    {
        return speaker == character;
              
    }

    public void Show() 
    {
        gameObject.SetActive(true);
   
    }

    public void Hide() 
    {

        gameObject.SetActive(false);
    
    }
}
