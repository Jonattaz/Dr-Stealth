using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Papae.UnitySDK.Managers;


namespace PudimdimGames{
    public class QTE_Event : MonoBehaviour{

        [SerializeField] private AudioClip lockpickFailSound;

        [SerializeField] private float fillAmount = 0;
        [SerializeField] private float fillValue;
        private float timeThreshold = 0;
        
        [SerializeField] private GameObject door;

        [SerializeField] private GameObject tubulacao;
        
        [SerializeField] private AudioClip lockpickingSound;

        [SerializeField] private bool isDoor;

        [SerializeField] private float fillHold;
        

        
        // Update is called once per frame
        void Update()
        {
            if(UnityEngine.Input.GetKeyDown("e")){
                fillAmount += fillValue;
                AudioManager.Instance.PlaySFX(lockpickingSound, 1f);
            }

            timeThreshold += Time.deltaTime;    

            if(timeThreshold > .1){
                timeThreshold = 0;
                fillAmount -= fillHold;
            }

            if(CountDownTimer.TimerInstance.noise){
                AudioManager.Instance.PlaySFX(lockpickFailSound, 1f);
            }

            if(fillAmount < 0){
                fillAmount = 0;
            }

            if(fillAmount >= 1){
                if(isDoor){
                    door.GetComponent<Door>().canOpenGet = true;
                }else{
                    tubulacao.GetComponent<Teleporting>().canTeleport = true;
                }
            }

            GetComponent<Image>().fillAmount = fillAmount;
        }
    }
}
















