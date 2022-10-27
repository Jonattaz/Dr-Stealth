using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames
{
    public enum CharacterStance{Standing, Crouching, Proning}
    public class Comp_CharacterController : MonoBehaviour
    {

        public static Vector3 playerPos;

        [Header("Speed (Normal, Sprinting)")]
        [SerializeField] private Vector2 _standingSpeed = new Vector2(0,0);
        [SerializeField] private Vector2 _crouchingSpeed = new Vector2(0,0);
        [SerializeField] private Vector2 _proningSpeed = new Vector2(0,0);

        [Header("Capsule (Radius, Height, YOffset)")]
        [SerializeField] private Vector3 _standingCapsule = Vector3.zero;
        [SerializeField] private Vector3 _crouchingCapsule = Vector3.zero;
        [SerializeField] private Vector3 _proningCapsule = Vector3.zero;

        [Header("Sharpness")]
        [SerializeField] private float _standingRotationSharpness = 10f;
        [SerializeField] private float _crouchingRotationSharpness = 10f;
        [SerializeField] private float _proningRotationSharpness = 10f;
        [SerializeField] private float _moveSharpness = 10f;

        private Animator _animator;
        private CapsuleCollider _collider;
        private Comp_PlayerInputs _inputs;

        private bool _proning;
        private float _runSpeed;
        private float _sprintSpeed;
        private float _rotationSharpness;
        private LayerMask _layerMask;
        private CharacterStance _stance;
        private Collider[] _obstructions = new Collider[8];
    
        private float _targetSpeed;
        private Quaternion _targetRotation;

        private float _newSpeed;
        private Vector3 _newVelocity;
        private Quaternion _newRotation;

        // Animator state names
        private const string _standToCrouch = "Base Layer.Base Crouching";
        private const string _standToProne = "Base Layer.Stand To Prone";
        private const string _crouchToStand = "Base Layer.Base Standing";
        private const string _crouchToProne = "Base Layer.Crouch To Prone";
        private const string _proneToStand = "Base Layer.Prone To Stand";
        private const string _proneToCrouch = "Base Layer.Prone To Crouch";

    
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider>();
            _inputs = GetComponent<Comp_PlayerInputs>(); 
            
            // Set defaults
            _runSpeed = _standingSpeed.x;
            _sprintSpeed = _standingSpeed.y;
            _rotationSharpness = _standingRotationSharpness;
            _stance = CharacterStance.Standing;
            SetCapsuleDimensions(_standingCapsule);


            int _mask = 0;
            for (int i = 0; i < 32; i++){   
                if(!(Physics.GetIgnoreLayerCollision(gameObject.layer, i))){
                    _mask |= 1 << i;
                }
            }
            _layerMask = _mask;
            _animator.applyRootMotion = false;

            StartCoroutine(TrackTarget());
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
            StaceControl();

        }

        public bool RequestStanceChange(CharacterStance newStance){
            if(_stance == newStance)
                return true;

            switch(_stance){
                case CharacterStance.Standing:
                    if(newStance == CharacterStance.Crouching){
                            _runSpeed = _crouchingSpeed.x;
                            _sprintSpeed = _crouchingSpeed.y;
                            _rotationSharpness = _crouchingRotationSharpness;
                            _stance= newStance;
                            _animator.CrossFadeInFixedTime(_standToCrouch, 0.25f);
                            SetCapsuleDimensions(_crouchingCapsule);
                            return true;
                    }else if(newStance == CharacterStance.Proning){    
                            _newSpeed = 0;
                            _proning = true;
                            _animator.SetFloat("Forward", 0);
                            _runSpeed = _proningSpeed.x;
                            _sprintSpeed = _proningSpeed.y;
                            _rotationSharpness = _proningRotationSharpness;
                            _stance = newStance;
                            _animator.CrossFadeInFixedTime(_standToProne, 0.25f);
                            SetCapsuleDimensions(_proningCapsule);
                            return true;
                    }
                    break;
                case CharacterStance.Crouching:
                    if(newStance == CharacterStance.Standing){
                            _runSpeed = _standingSpeed.x;
                            _sprintSpeed = _standingSpeed.y;
                            _rotationSharpness = _standingRotationSharpness;
                            _stance= newStance;
                            _animator.CrossFadeInFixedTime(_crouchToStand, 0.25f);
                            SetCapsuleDimensions(_standingCapsule);
                            return true;
                    }else if(newStance == CharacterStance.Proning){
                            _newSpeed = 0;
                            _proning = true;
                            _animator.SetFloat("Forward", 0);
                            _runSpeed = _proningSpeed.x;
                            _sprintSpeed = _proningSpeed.y;
                            _rotationSharpness = _proningRotationSharpness;
                            _stance = newStance;
                            _animator.CrossFadeInFixedTime(_crouchToProne, 0.25f);
                            SetCapsuleDimensions(_proningCapsule);
                            return true;
                    }
                    break;
                
                case CharacterStance.Proning:
                    if(newStance == CharacterStance.Standing){ 
                            _newSpeed = 0;
                            _proning = true;
                            _animator.SetFloat("Forward", 0);
                            _runSpeed = _standingSpeed.x;
                            _sprintSpeed = _standingSpeed.y;
                            _rotationSharpness = _standingRotationSharpness;
                            _stance= newStance;
                            _animator.CrossFadeInFixedTime(_proneToStand, 0.5f);
                            SetCapsuleDimensions(_standingCapsule);
                            return true;
                    }else if(newStance == CharacterStance.Crouching){
                            _newSpeed = 0;
                            _proning = true;
                            _animator.SetFloat("Forward", 0);
                            _runSpeed = _crouchingSpeed.x;
                            _sprintSpeed = _crouchingSpeed.y;
                            _rotationSharpness = _crouchingRotationSharpness;
                            _stance = newStance;
                            _animator.CrossFadeInFixedTime(_proneToCrouch, 0.5f);
                            SetCapsuleDimensions(_crouchingCapsule);
                            return true;
                    }
                    break;
            }
            return false;
        }

        private bool CharacterOverlap(Vector3 dimensions){

            float _radius = dimensions.x;
            float _height = dimensions.y;
            Vector3 _center = new Vector3(_collider.center.x, dimensions.z, _collider.center.z);

            Vector3 _point0;
            Vector3 _point1;

            if(_height < _radius * 2){
                _point0 = transform.position + _center;
                _point1 = transform.position + _center;
            }else{
                _point0 = transform.position + _center + (transform.up *(_height * 0.5f - _radius));
                _point1 = transform.position + _center - (transform.up *(_height * 0.5f - _radius));
            }

            int _numOverLaps = Physics.OverlapCapsuleNonAlloc(_point0, _point1, _radius, _obstructions, _layerMask);
            for(int i = 0; i < _numOverLaps; i++){
                if(_obstructions[i] == _collider)
                    _numOverLaps--;
            }
            return _numOverLaps > 0;
        }

        private void SetCapsuleDimensions(Vector3 dimensions){
            _collider.center = new Vector3(_collider.center.x, dimensions.z, _collider.center.z);
            _collider.radius = dimensions.x;
            _collider.height = dimensions.y;
        }

        public void OnSMBEvent(string eventName){
            switch (eventName){
                case "ProneEnd":
                    _proning = false;
                    break;
                default:
                    break;
            }
        }

        private void Movement(){
            
            if(_proning){
                return;
            }

            // Input Handle
            Vector3 _moveInputVector = new Vector3(_inputs.MoveAxisRightRaw, 0, _inputs.MoveAxisForwardRaw).normalized;
            
            // Move Speed
            if(_inputs.Sprint.Pressed()){ _targetSpeed = _moveInputVector != Vector3.zero ? _sprintSpeed : 0; }
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

        private void StaceControl(){
              
            if(_proning){
                return;
            }
            switch (_stance){
                case CharacterStance.Standing:
                    if(_inputs.Crouching.PressedDown()){RequestStanceChange(CharacterStance.Crouching);}
                    else if(_inputs.Proning.Pressed()){ RequestStanceChange(CharacterStance.Proning);}
                    break;
                case CharacterStance.Crouching:
                    if(_inputs.Crouching.PressedDown()){RequestStanceChange(CharacterStance.Standing);}
                    else if(_inputs.Proning.Pressed()){ RequestStanceChange(CharacterStance.Proning);}
                    break;
                case CharacterStance.Proning:
                    if(_inputs.Crouching.PressedDown()){RequestStanceChange(CharacterStance.Crouching);}
                    else if(_inputs.Proning.Pressed()){ RequestStanceChange(CharacterStance.Standing);}
                    break;
            }
        }

        IEnumerator TrackTarget(){
            while(true){
                playerPos = gameObject.transform.position;
                yield return null;
            }
        }

    }
}



























