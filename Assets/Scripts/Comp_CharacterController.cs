using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames
{

    public class Comp_CharacterController : MonoBehaviour
    {
        private Animator _animator;
        private Comp_PlayerInputs _inputs;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _inputs = GetComponent<Comp_PlayerInputs>();

            _animator.applyRootMotion = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

    }
}