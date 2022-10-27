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

        [SerializeField] private KeyCode menuButton;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            MenuActivator();   
        }

        private void MenuActivator(){
            if(UnityEngine.Input.GetKeyDown(menuButton)){
                if(mainCam.activeInHierarchy){
                    // Ativar menu

                    mainCam.SetActive(false);
                    menu.SetActive(true);
                }else{
                    // Desativar menu
                    menu.SetActive(false);
                    mainCam.SetActive(true);
                }
            }
        }

    	public void LoadMenu(){
			if(sceneName != ""){
				SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}
		}

    }

}