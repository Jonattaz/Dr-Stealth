using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    public static CheatController cheatInstance;
    [SerializeField] private bool setCheat;
    [HideInInspector] public bool getCheat;
    [SerializeField] private GameObject cheatLine;
    [SerializeField] private GameObject normalLine;
    [SerializeField] private bool created;

    void Awake ()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start(){
        cheatInstance = this;
        
    }

    
    public void CheatOn(){
        setCheat = true;
        getCheat = setCheat;
        normalLine.SetActive(false);
        cheatLine.SetActive(true);
    }

    public void CheatOff(){
        setCheat = false;
        getCheat = setCheat;
        cheatLine.SetActive(false);
        normalLine.SetActive(true);
    }

}
