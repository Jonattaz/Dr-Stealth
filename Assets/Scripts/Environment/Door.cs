using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Papae.UnitySDK.Managers;

namespace PudimdimGames{

    public class Door: MonoBehaviour
    {
        public GameObject swivelAxis;

        public GameObject QTE;

        [SerializeField] private AudioClip doorSound;

        [SerializeField] private bool canOpen;
        [SerializeField] private bool makeDoorSound;

        [SerializeField] private GameObject needKeyText;        
        
        public bool canOpenGet;

        public bool key;

        public bool needKey;

        private Animator swivelAnnimation;

        
        // Start is called before the first frame update
        void Start()
        {
           swivelAnnimation = swivelAxis.GetComponent<Animator>();   
        }

        void OnTriggerEnter(Collider other)
        {   
            if(!needKey){
                if(!canOpenGet )
                    QTE.SetActive(true);
                else{
                    QTE.SetActive(false);
                    CountDownTimer.TimerInstance.canCount = false;
                    CountDownTimer.TimerInstance.restart = true;
                    CountDownTimer.TimerInstance.noise = false;
                }
                
                canOpen = canOpenGet;
                CountDownTimer.TimerInstance.canCount = true;
            }else{     
                if(key){
                    canOpenGet = true;
                }else{                
                    if(other.gameObject.CompareTag("Player")){
                        needKeyText.SetActive(true);
                    }
                }
            }
            
            if (UnityEngine.Input.GetKey("e") && canOpen ){
                swivelAnnimation.SetBool("buttonDown", true);
            }

           
    
        }


        void OnTriggerStay(Collider other)
        {
            if(!CountDownTimer.TimerInstance.noise){
                canOpen = canOpenGet;
                
                if (UnityEngine.Input.GetKey("e") && canOpen){
                    swivelAnnimation.SetBool("buttonDown", true);
                }
            }
            
        }


        void OnTriggerExit(Collider other)
        {
            QTE.SetActive(false);
            CountDownTimer.TimerInstance.canCount = false;
            CountDownTimer.TimerInstance.restart = true;
            CountDownTimer.TimerInstance.noise = false;
            canOpen = canOpenGet;
            swivelAnnimation.SetBool("buttonDown", false);
            swivelAnnimation.SetBool("buttonDown", false);
            if(other.gameObject.CompareTag("Player")){
                needKeyText.SetActive(false);
            }        

        }

    }

}
