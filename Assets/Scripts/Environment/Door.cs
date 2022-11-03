using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames{

    public class Door: MonoBehaviour
    {

        public GameObject swivelAxis;

        [SerializeField] public GameObject QTE;

        [SerializeField] bool canOpen;
        
        public bool canOpenGet;

        Animator swivelAnnimation;

         bool buttonStatus;

        bool buttonMessage;
        
        // Start is called before the first frame update
        void Start()
        {
           swivelAnnimation = swivelAxis.GetComponent<Animator>();    
        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnTriggerEnter(Collider other)
        {
            if(!canOpenGet)
                QTE.SetActive(true);
            else{
                QTE.SetActive(false);
            }
            
            canOpen = canOpenGet;
            
            buttonMessage = true;
            CountDownTimer.TimerInstance.canCount = true;

            
            if (UnityEngine.Input.GetKey("e") && canOpen ){
                swivelAnnimation.SetBool("buttonDown", true);
            }
    
        }


        void OnTriggerStay(Collider other)
        {
            if(!CountDownTimer.TimerInstance.noise){
                canOpen = canOpenGet;
                
                buttonMessage = true;
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
            buttonMessage = false;
            swivelAnnimation.SetBool("buttonDown", false);
        }

    }

}
