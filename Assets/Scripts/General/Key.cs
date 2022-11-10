using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames
{

    public class Key : MonoBehaviour
    {
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject keyText;
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        void OnTriggerEnter(Collider other)
        {

            if(other.gameObject.CompareTag("Player")){
                keyText.SetActive(true);
                if(UnityEngine.Input.GetKeyUp(KeyCode.E)){
                    keyText.SetActive(false);
                    Debug.Log("Porra");
                }
            }
        
        }

    
        void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag("Player")){
                keyText.SetActive(true);
                if(UnityEngine.Input.GetKeyUp(KeyCode.E)){
                    keyText.SetActive(false);
                    door.GetComponent<Door>().key = true; 

                    this.gameObject.SetActive(false);
                }
            }
        
        }

        void OnTriggerExit(Collider other){
            if(other.gameObject.CompareTag("Player")){
                keyText.SetActive(false);
            }        
        }

    }
}