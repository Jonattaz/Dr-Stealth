using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace PudimdimGames{
    

    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainCam;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject deathMenu;
        
        [SerializeField] private GameObject actionHUD;

        [SerializeField] private string menuScene;
        [SerializeField] private string reloadScene;
        [HideInInspector] public bool getCaughtInfo;
        [SerializeField] private bool setCaughtInfo; 

        [SerializeField] private bool isPaused;
        [SerializeField] private KeyCode menuButton;
        public static GameMenu gameMenuInstance;

        void Start(){
            gameMenuInstance = this;
            Time.timeScale = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if(setCaughtInfo){
                PlayerDead();
            }else{
                MenuActivator();
                setCaughtInfo = getCaughtInfo;
            }  
        }

        private void MenuActivator(){
            if(UnityEngine.Input.GetKeyDown(menuButton)){
                if(mainCam.activeInHierarchy){
                    // Ativar menu
                    isPaused = false;
                    actionHUD.SetActive(false);
                    mainCam.SetActive(false);
                    menu.SetActive(true);
                }else{
                    isPaused = true;
                    // Desativar menu
                    actionHUD.SetActive(true);
                    menu.SetActive(false);
                    mainCam.SetActive(true);
                }
                PauseGame();
            }
        }

    	public void LoadMenu(){
			if(menuScene != ""){
                Time.timeScale = 1;
				SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
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

        public void ReloadGame(){
            if(reloadScene != ""){
				SceneManager.LoadScene(reloadScene, LoadSceneMode.Single);
			}
        }

        public void PlayerDead(){
            Time.timeScale = 0;
            mainCam.SetActive(false);
            menu.SetActive(false);
            deathMenu.SetActive(true);
        }

    }

}