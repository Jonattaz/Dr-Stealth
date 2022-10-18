using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PudimdimGames {
    
    public class CountDownTimer : MonoBehaviour
    {
        [SerializeField] float currentTime = 0f;
        [SerializeField] float startingTime = 10f;

        [SerializeField] Text countDownText;

        [SerializeField] GameObject QTE;

        public static CountDownTimer TimerInstance;

        [HideInInspector] public bool restart;

        [HideInInspector] public bool noise;

        [HideInInspector] public bool canCount;
        
        /// Awake is called when the script instance is being loaded.
        void Awake(){
            TimerInstance = this;
        }

        // Start is called before the first frame update
        void Start(){
            restart = false;
            noise = false;
            currentTime = startingTime;
        }

        // Update is called once per frame
        void Update()
        {
            if(canCount){
                CountDownController();
            }else{
                currentTime = startingTime;
            }
        }

        // TimerController
        void CountDownController(){
           
            currentTime -= 1 * Time.deltaTime;
            countDownText.text = currentTime.ToString("0");

            if(currentTime <= 0 && !restart){
                QTE.SetActive(false);
                noise = true;
                currentTime = 0;
            } else if(restart) {
                currentTime = startingTime;
                restart = !restart;
            }
        }

    }
}