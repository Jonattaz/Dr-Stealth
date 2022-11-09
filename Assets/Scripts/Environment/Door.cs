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
        
        public bool canOpenGet;

        private Animator swivelAnnimation;

        
        // Start is called before the first frame update
        void Start()
        {
           swivelAnnimation = swivelAxis.GetComponent<Animator>();    
        }

        void OnTriggerEnter(Collider other)
        {
            if(!canOpenGet)
                QTE.SetActive(true);
            else{
                QTE.SetActive(false);
                CountDownTimer.TimerInstance.canCount = false;
                CountDownTimer.TimerInstance.restart = true;
                CountDownTimer.TimerInstance.noise = false;
            }
            
            canOpen = canOpenGet;
            CountDownTimer.TimerInstance.canCount = true;

            
            if (UnityEngine.Input.GetKey("e") && canOpen ){
                swivelAnnimation.SetBool("buttonDown", true);
                AudioManager.Instance.PlaySFX(doorSound, 1f);
            }
    
        }


        void OnTriggerStay(Collider other)
        {
            if(!CountDownTimer.TimerInstance.noise){
                canOpen = canOpenGet;
                
                if (UnityEngine.Input.GetKey("e") && canOpen){
                    swivelAnnimation.SetBool("buttonDown", true);
                    AudioManager.Instance.PlaySFX(doorSound, 10f);
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
        }

    }

}
