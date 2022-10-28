using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace PudimdimGames{
    

    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainCam;
        [SerializeField] private GameObject menu;

        [SerializeField] private string sceneName;

        [SerializeField] private bool isPaused;
        [SerializeField] private KeyCode menuButton;

        // Update is called once per frame
        void Update()
        {
            MenuActivator();   
        }

        private void MenuActivator(){
            if(UnityEngine.Input.GetKeyDown(menuButton)){
                if(mainCam.activeInHierarchy){
                    // Ativar menu
                    isPaused = false;
                    mainCam.SetActive(false);
                    menu.SetActive(true);
                }else{
                    isPaused = true;
                    // Desativar menu
                    menu.SetActive(false);
                    mainCam.SetActive(true);
                }
                PauseGame();
            }
        }

    	public void LoadMenu(){
			if(sceneName != ""){
				SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}
		}

        private void PauseGame(){
            if(isPaused){
                Time.timeScale = 1;
                isPaused = false;
            }else{
                Time.timeScale = 0;
                isPaused = true;
            }
        }

    }

}