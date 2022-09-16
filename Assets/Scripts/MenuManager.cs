using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Controles Canvas
    public GameObject opcoes;

    // Menu Canvas
    public GameObject menu;

    // Credits 
    public GameObject credits;

    public GameObject intro;
  
   [SerializeField]
    // Musica que toca no menu
    private AudioClip menuSoundtrack;
    
    public static MenuManager MenuInstance;

    private void Awake(){
       MenuInstance = this;
       DontDestroyOnLoad(transform.gameObject);
    }

    // Load another scenes
    public void SceneLoad(int Scene)
    {
        SceneManager.LoadScene(Scene);
        
    }

    // Allows the player to exit the game
    public void ExitGame()
    {
        Application.Quit();
    }


    // When the Options button is clicked, activate the options canvas and deactivate the menu canvas
    public void MenuToOpcoes()
    {
        menu.SetActive(false);
        opcoes.SetActive(true);

    }


    // When the Menu button is clicked, activate the Menu canvas and deactivate the Options canvas
    public void OpcoesToMenu()
    {
        opcoes.SetActive(false);
        menu.SetActive(true);
    }

    
    public void MenuToCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void MenuToIntro()
    {
        menu.SetActive(false);
        intro.SetActive(true);
    }


    public void CreditsToMenu()
    {
        credits.SetActive(false);
        menu.SetActive(true);
    }
}
