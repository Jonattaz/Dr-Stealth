using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatController : MonoBehaviour
{
    public static CheatController cheatInstance;
    [SerializeField] private bool setCheat;
    [HideInInspector] public bool getCheat;
    [SerializeField] private GameObject cheatLine;
    [SerializeField] private GameObject normalLine;
    private bool created;
     public bool canLoad;

    void Awake (){
        DontDestroyOnLoad (this);
            
        if (cheatInstance == null) {
            cheatInstance = this;
        }else {
            Destroy(gameObject);
        }
        
    }


    void Update(){
        normalLine = GameObject.Find("Game_Menu/Menu/Canv_Settings/PANELS/PanelGame/NormalLine");
        cheatLine = GameObject.Find("Game_Menu/Menu/Canv_Settings/PANELS/PanelGame/CheatLine");        
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
