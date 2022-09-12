using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Problema da animação. Prone to stand e prone to crouch

namespace  PudimdimGames{
    
    public class Comp_SMBEventCurrator : MonoBehaviour
    {
        [SerializeField] private bool m_debug = false;
        [SerializeField] public UnityEvent<string> m_event = new UnityEvent<string>();
        public UnityEvent<string> Event{get => m_event;}

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            m_event.AddListener(OnSMBEvent);
        }

        private void OnSMBEvent(string eventName){
            if(m_debug){
                Debug.Log(eventName);
            }
        }


    }

}
