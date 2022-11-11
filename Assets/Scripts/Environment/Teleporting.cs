using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames{
    
    public class Teleporting : MonoBehaviour
    {
        [SerializeField] private Transform teleportTarget;
        [SerializeField] private GameObject player;
        [SerializeField] public bool canTeleport;
        public GameObject QTE;

        [SerializeField] private bool hasQTE;  

        void OnTriggerEnter(Collider other){
             
            

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

       void OnTriggerStay(Collider other){
            if(canTeleport){
                StartCoroutine(Teleport());
                CameraFade.FadeInstance.Fade();
                player.GetComponent<Comp_CharacterController>().enabled = false;
            }

        }
   
         /// <summary>
         /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
         /// </summary>
         /// <param name="other">The other Collider involved in this collision.</param>
         void OnTriggerExit(Collider other){
            QTE.SetActive(false);
            CountDownTimer.TimerInstance.canCount = false;
            CountDownTimer.TimerInstance.restart = true;
            CountDownTimer.TimerInstance.noise = false;
         }

        IEnumerator Teleport(){
            yield return new WaitForSeconds(1);
            player.transform.position = new Vector3(
                teleportTarget.transform.position.x,
                teleportTarget.transform.position.y,
                teleportTarget.transform.position.z
            );
            player.GetComponent<Comp_CharacterController>().enabled = true;
            
        }
        
    }
}