using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PudimdimGames{
        
    public class Introduction : MonoBehaviour
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private string sceneName;

        [SerializeField] private float duration = 5f;
        private float elapsedTime = 0;
        private float t;

        // Update is called once per frame
        void Update()
        {
            ScreenMove();
        }
        
        public void ScreenMove(){
            elapsedTime += Time.deltaTime;
            t = elapsedTime / duration;

            transform.position = Vector3.Lerp(pointB.position, pointA.position, t);
        }

        public void NewGame(){
            if(sceneName != ""){
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
    }
}