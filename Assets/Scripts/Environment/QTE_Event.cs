using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PudimdimGames{
    public class QTE_Event : MonoBehaviour{

        [SerializeField] private float fillAmount = 0;
        [SerializeField] private float fillValue;
        private float timeThreshold = 0;
        [SerializeField] private GameObject door;

        [SerializeField] private GameObject tubulacao;
        
        [SerializeField] private bool isDoor;

        [SerializeField] private float fillHold;
         
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(UnityEngine.Input.GetKeyDown("e")){
                fillAmount += fillValue;
            }

            timeThreshold += Time.deltaTime;    

            if(timeThreshold > .1){
                timeThreshold = 0;
                fillAmount -= fillHold;
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
















