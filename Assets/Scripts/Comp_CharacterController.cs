using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames
{

    public class Comp_CharacterController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _runSpeed = 6f;
        [SerializeField] private float _sprintSpeed = 8f;

        [Header("Sharpness")]
        [SerializeField] private float _rotationSharpness = 10f;
        [SerializeField] private float _moveSharpness = 10f;

        private Animator _animator;
        private Comp_PlayerInputs _inputs;

        private float _targetSpeed;
        private Quaternion _targetRotation;

        private float _newSpeed;
        private Vector3 _newVelocity;
        private Quaternion _newRotation;

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
            // Input Handle
            Vector3 _moveInputVector = new Vector3(_inputs.MoveAxisRightRaw, 0, _inputs.MoveAxisForwardRaw).normalized;
            
            // Move Speed
            if(_inputs.Sprint.Pressed()){  _targetSpeed = _moveInputVector != Vector3.zero ? _sprintSpeed : 0; }
            else                        {  _targetSpeed = _moveInputVector != Vector3.zero ? _runSpeed : 0; }
            _newSpeed = Mathf.Lerp(_newSpeed, _targetSpeed, Time.deltaTime * _moveSharpness);
            
            // Velocity
            _newVelocity = _moveInputVector * _newSpeed;
            transform.Translate(_newVelocity * Time.deltaTime, Space.World);

            // Rotation
            if(_targetSpeed != 0){
                _targetRotation = Quaternion.LookRotation(_moveInputVector);
                _newRotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * _rotationSharpness);
                transform.rotation = _newRotation;
            }

            

            // Animations
            _animator.SetFloat("Forward", _newSpeed);
            
        }

    }
}











