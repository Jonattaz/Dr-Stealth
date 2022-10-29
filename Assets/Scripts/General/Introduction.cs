using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PudimdimGames{
        
    public class Introduction : MonoBehaviour
    {
        [SerializeField] private Transform[] pointA;
        [SerializeField] private Transform[] pointB;
        [SerializeField] private string sceneName;
        
        [SerializeField] private float[] duration;

        [SerializeField] private GameObject[] texts;

        [SerializeField] private int index;
        [SerializeField] private int actualIndex;
        private float elapsedTime = 0;
        private float t;

        void Update(){
            if(index == actualIndex)
                ScreenMove();
        }

        public void ScreenMove(){
            if(texts[index].gameObject.activeInHierarchy){    
                elapsedTime += Time.deltaTime;
                t = elapsedTime / duration[index];

                texts[index].transform.position = Vector3.Lerp(pointB[index].position, pointA[index].position, t);
            }
        }

        public void NewGame(){
            if(sceneName != ""){
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }

        public void nextText(){
            texts[index].SetActive(false);
            index++;
            texts[index].SetActive(true);
            elapsedTime = 0;
            actualIndex = index;
        }


    }
}