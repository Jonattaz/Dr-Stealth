using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames
{
    [System.Serializable]
    public class Input
    {   
        public KeyCode primary;
        public KeyCode alternate;

        public bool Pressed(){
            return UnityEngine.Input.GetKey(primary) || UnityEngine.Input.GetKey(alternate);
        }

        public bool PressedDown(){
            return UnityEngine.Input.GetKeyDown(primary) || UnityEngine.Input.GetKeyDown(alternate);
        }

        public bool PressedUp(){
            return UnityEngine.Input.GetKeyUp(primary) || UnityEngine.Input.GetKeyUp(alternate);
        }

    }

    public class Comp_PlayerInputs : MonoBehaviour
    {
        public Input Forward;
        public Input Backward;
        public Input Right;
        public Input Left;
        public Input Sprint;

        

    }
}


























