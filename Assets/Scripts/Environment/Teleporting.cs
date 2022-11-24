using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PudimdimGames{
    
    public class Teleporting : MonoBehaviour
    {
        [SerializeField] private bool outOfBound;
        [SerializeField] private Transform teleportTarget;
        [SerializeField] private GameObject player;
        public bool canTeleport;
        public GameObject QTE;
        [SerializeField] private bool endGame;
        [SerializeField] private bool hasQTE;  
        [SerializeField] private string EndGameScene;
        
        
        void OnTriggerEnter(Collider other){
             if(outOfBound){
                CameraFade.FadeInstance.Fade();
                StartCoroutine(Teleport());
            }

            if(!outOfBound){    
                if(!canTeleport && hasQTE){
                    QTE.SetActive(true);
                    CountDownTimer.TimerInstance.canCount = true;
                }else{
                    QTE.SetActive(false);
                    CountDownTimer.TimerInstance.canCount = false;
                    CountDownTimer.TimerInstance.restart = true;
                    CountDownTimer.TimerInstance.noise = false;
                }
            }
        }

       void OnTriggerStay(Collider other){
            if(canTeleport){
               if(endGame){
                    CameraFade.FadeInstance.Fade();
                    player.GetComponent<Comp_CharacterController>().enabled = false;
                    StartCoroutine(LoadEndGame());
               }else{
                    StartCoroutine(Teleport());
                    CameraFade.FadeInstance.Fade();
                    player.GetComponent<Comp_CharacterController>().enabled = false;
               }                
            }
        }
         /// <summary>
         /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
         /// </summary>
         /// <param name="other">The other Collider involved in this collision.</param>
         void OnTriggerExit(Collider other){
           if(!outOfBound){
                QTE.SetActive(false);
                CountDownTimer.TimerInstance.canCount = false;
                CountDownTimer.TimerInstance.restart = true;
                CountDownTimer.TimerInstance.noise = false;
           }
         }

        IEnumerator Teleport(){
            yield return new WaitForSeconds(1);
             if(outOfBound)
                CameraFade.FadeInstance.Fade();
            player.transform.position = new Vector3(
                teleportTarget.transform.position.x,
                teleportTarget.transform.position.y,
                teleportTarget.transform.position.z
            );
            player.GetComponent<Comp_CharacterController>().enabled = true;
            
        }
        
        
        IEnumerator LoadEndGame(){
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(EndGameScene, LoadSceneMode.Single);
                   
        }
    

    }
}