using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PudimdimGames;

namespace PudimdimGames{

    public class DetectionSystem : MonoBehaviour
    {
        [HideInInspector]
        public int idController;

        public string stateHolder;
        
        // 0 = Patrol, 1 = Chase, 2 = Heard a noise, 3 = You got caught
        [SerializeField] private int IconId;

        [SerializeField] private GameObject[] detectionIcons;
        [SerializeField] private GameObject aiLocal;

        // Start is called before the first frame update
        void Start()
        {
            detectionIcons[IconId] = detectionIcons[0];   
        }

        // Update is called once per frame
        void Update()
        {
            IconChanger();
        }

        void IconChanger(){

            stateHolder = aiLocal.GetComponent<AI_Enemy>().stateText;

            switch (stateHolder)
            {
                case "Patrol Mode":
                    idController = 0;
                    break;
                case "Chase Mode":
                    idController = 1;
                    break;
                case "Enemy Heard a Noise":
                    idController = 2;
                    break;
                case "You Got Caught":
                    idController = 3;
                    break;
            }

            if(idController != IconId){
                detectionIcons[IconId].SetActive(false);
                IconId = idController;
                detectionIcons[idController].SetActive(true);
            }

        }

    }
}
